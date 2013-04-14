using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coombi125565.Models
{
    [Table("Likes")]
    public class LikeModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }
        public int UserId { get; set; }
        //public int PostId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}