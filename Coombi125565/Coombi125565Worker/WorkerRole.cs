using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.IO;
using System.Drawing;

namespace Coombi125565Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Trace.WriteLine("Coombi WorkerRole called", "Information");

            //On recupère la queue
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageCS"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("coombiqueue");
            queue.CreateIfNotExists();

            //Accès aux blobs
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("coombimage");

            CloudQueueMessage msg = queue.GetMessage();
            //Si un message est recu ...
            if (msg != null)
            {
                string path = msg.AsString;
                string thumbnailName = System.IO.Path.GetFileNameWithoutExtension(path) + ".jpeg";
                Trace.TraceInformation(string.Format("Dequeued '{0}'", path));
                CloudBlockBlob content = container.GetBlockBlobReference(path);//Image originale
                CloudBlockBlob thumbnail = container.GetBlockBlobReference("thumb" + thumbnailName);//Image en thumbnail
                MemoryStream image = new MemoryStream();

                content.DownloadToStream(image);

                image.Seek(0, SeekOrigin.Begin);

                thumbnail.Properties.ContentType = "image/jpeg";
                thumbnail.UploadFromStream(CreateThumbnail(image));

                Trace.TraceInformation(string.Format("Done with '{0}'", path));

                queue.DeleteMessage(msg);
            }
            else
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        /**
         * Permet de creer un thumbnail d'une image (128x128)
         */
        private Stream CreateThumbnail(Stream input)
        {
            Bitmap orig = new Bitmap(input);

            int width;
            int height;
            if (orig.Width > orig.Height)
            {
                width = 128;                                                
                height = 128 * orig.Height / orig.Width;
            }
            else
            {
                height = 128;
                width = 128 * orig.Width / orig.Height;
            }

            Bitmap thumb = new Bitmap(width, height);
            using (Graphics graphic = Graphics.FromImage(thumb))
            {
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                graphic.DrawImage(orig, 0, 0, width, height);
                MemoryStream ms = new MemoryStream();
                thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }
    }
}
