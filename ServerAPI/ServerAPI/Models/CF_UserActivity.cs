using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.CF_Models
{
    public class Comment
    {
        public long userId { get; set; }
        public long postId { get; set; }
        public System.DateTime date { get; set; }
        public string content { get; set; }

        public virtual GeneralPost GeneralPost { get; set; }
        public virtual User User { get; set; }
    }
    public class Report
    {
        public long userId { get; set; }
        public long photoId { get; set; }
        public System.DateTime date { get; set; }
        public string reason { get; set; }

        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
    }
    public class Sharing
    {
        public long userId { get; set; }
        public long generalPostId { get; set; }
        public long sharedFamilyId { get; set; }
        public System.DateTime date { get; set; }
        public string message { get; set; }

        public virtual Family Family { get; set; }
        public virtual GeneralPost GeneralPost { get; set; }
        public virtual User User { get; set; }
    }
}