using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coombi125565.Models
{
    [Table("Comment")]
    public class CommentModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int UserId { get; set; }
        /*public int PostId { get; set; }*/

        [DataType(DataType.Text)]
        public String Content { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}