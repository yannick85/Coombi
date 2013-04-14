using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coombi125565.Models
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> Users { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<GroupMembershipModel> GroupMemberships { get; set; }
        public DbSet<FollowModel> Follows { get; set; }
    }

    [Table("Post")]
    public class PostModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        public int UserId { get; set; }
        [DataType(DataType.Text)]
        public String Content { get; set; }
        public String Picture { get; set; }
        public DateTime Time { get; set; }
        public int GroupId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }

        public virtual List<CommentModel> Comments { get; set; }
        public virtual List<LikeModel> Likes { get; set; }
    }

    public class AddPostModel
    {
        [Required]
        [Display(Name = "Content")]
        [System.ComponentModel.DataAnnotations.StringLength(142)]
        public string Content { get; set; }
        public HttpPostedFileBase Picture { get; set; }
    }
}