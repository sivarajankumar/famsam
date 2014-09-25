using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famsam.serverapi.Models
{
    public class Sharing
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public long UserId { get; set; }
        [Key, ForeignKey("Family"), Column(Order = 1)]
        public long FamilyId { get; set; }
        [Key, ForeignKey("GeneralPost"), Column(Order = 2)]
        public long GeneralPostId { get; set; }

        public User User { get; set; }
        public Family Family { get; set; }
        public GeneralPost GeneralPost { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }

    public class Like
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public long UserId { get; set; }
        [Key, ForeignKey("GeneralPost"), Column(Order = 1)]
        public long GeneralPostId { get; set; }

        public User User { get; set; }
        public GeneralPost GeneralPost { get; set; }
    }

    public class Comment
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public long UserId { get; set; }
        [Key, ForeignKey("GeneralPost"), Column(Order = 1)]
        public long GeneraPostId { get; set; }

        public User User { get; set; }
        public GeneralPost GeneralPost { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
    }

    public class FamilyFollowing
    {
        [Key, ForeignKey("Family"), Column(Order = 0)]
        public long FamilyId { get; set; }
        [Key, ForeignKey("User"), Column(Order = 1)]
        public long UserId { get; set; }

        [Key]
        public Family Family { get; set; }
        [Key]
        public User User { get; set; }
    }
}