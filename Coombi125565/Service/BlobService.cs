using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace Coombi125565.Service
{
    public class BlobService
    {
        public CloudBlobClient blobClient = null;
        public CloudBlobContainer container = null;

        public CloudQueueClient queueClient = null;
        public CloudQueue queue = null;

        public BlobService(String containerName = "coombimage", String queueName = "coombiqueue")
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageCS"));
            
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName);
            if (!container.Exists())
            {
                container.CreateIfNotExists();
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
        }

        public void deleteBlob(String name)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            blockBlob.Delete();
        }

        public String uploadBlobFromStream(String name, Stream file, String contentType = null, Boolean callQueue = true)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            if (contentType != null) {
                blockBlob.Properties.ContentType = contentType;
            }
            blockBlob.UploadFromStream(file);
            if (callQueue) {
                //Call to the worker role to execute a task on the blob (here : create the thumbnail of the image)
                queue.AddMessage(new CloudQueueMessage(blockBlob.Uri.ToString()));
            }

            //On retourne l'uri vers le blob
            return blockBlob.Uri.ToString();
        }
    }
}