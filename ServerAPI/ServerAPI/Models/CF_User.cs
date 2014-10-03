using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.CF_Models
{
    public class User
    {
        public User()
        {
            this.FamilyRole = new HashSet<FamilyRole>();
            this.NeighborRequest = new HashSet<NeighborRequest>();
            this.Report = new HashSet<Report>();
            this.Session = new HashSet<Session>();
            this.Sharing = new HashSet<Sharing>();
            this.Family = new HashSet<Family>();
        }

        public long id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string about { get; set; }
        public string workAt { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public string phone { get; set; }
        public Nullable<bool> infoPrivacy { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string avatarURL { get; set; }
        public string status { get; set; }
        //foreign key
        public string role { get; set; }
        public virtual ICollection<GeneralPost> CreatedPost { get; set; }
        public virtual ICollection<GeneralPost> LikedPost { get; set; }
        public virtual ICollection<FamilyRole> FamilyRole { get; set; }
        public virtual ICollection<NeighborRequest> NeighborRequest { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<Sharing> Sharing { get; set; }
        public virtual UserRole UserRole { get; set; }
        public virtual ICollection<Family> Family { get; set; }
    }
    public class UserRole
    {
        public const string LOGGED_IN_ROLE = "logged user";
        public const string ADMIN_ROLE = "admin";
        public UserRole()
        {
            this.User = new HashSet<User>();
        }

        public string rolename { get; set; }
        public string description { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
    public class Session
    {
        public string token { get; set; }
        public System.DateTime expired { get; set; }
        public virtual User User { get; set; }
    }
}