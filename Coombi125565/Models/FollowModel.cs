using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coombi125565.Models
{
    [Table("Follow")]
    public class FollowModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FollowId { get; set; }
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }
    }

    public class UserIndexModel
    {
        public List<int> Followed;
        public List<UserProfile> Users;
    }
}