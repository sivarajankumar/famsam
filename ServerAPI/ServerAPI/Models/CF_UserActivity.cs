using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.CF_Models
{
    public class Comment
    {
        public int userId { get; set; }
        public int postId { get; set; }
        public System.DateTime date { get; set; }
        public string content { get; set; }

        public virtual GeneralPost GeneralPost { get; set; }
        public virtual User User { get; set; }
    }
    public class Report
    {
        public int userId { get; set; }
        public int photoId { get; set; }
        public System.DateTime date { get; set; }
        public string reason { get; set; }

        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
    }
    public class Sharing
    {
        public int userId { get; set; }
        public int generalPostId { get; set; }
        public int sharedFamilyId { get; set; }
        public System.DateTime date { get; set; }
        public string message { get; set; }

        public virtual Family Family { get; set; }
        public virtual GeneralPost GeneralPost { get; set; }
        public virtual User User { get; set; }
    }
}