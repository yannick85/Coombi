using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coombi125565.Models
{
    [Table("Groups")]
    public class GroupModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        public int OwnerId { get; set; }
        public String Name { get; set; }

        [ForeignKey("OwnerId")]
        public virtual UserProfile Owner { get; set; }

        //[ForeignKey("GroupId")]
        //public virtual List<PostModel> Posts { get; set; }
    }

    public class AddGroupModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }

    public class DetailsGroup
    {
        public GroupModel Group;
        public List<PostModel> Posts;
        public List<UserProfile> Members;
    }

    public class ManageUsersGroup
    {
        public List<int> Members;
        public List<UserProfile> Users;
        public GroupModel Group;
    }
}