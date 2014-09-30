using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.CF_Models
{
    public class Family
    {
        public Family()
        {
            this.FamilyRole = new HashSet<FamilyRole>();
            this.Neighborhood = new HashSet<Neighborhood>();
            this.NeighborRequest = new HashSet<NeighborRequest>();
            this.Sharing = new HashSet<Sharing>();
            this.User = new HashSet<User>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public System.DateTime date { get; set; }
        public string coverURL { get; set; }
        public string description { get; set; }


        public virtual ICollection<FamilyRole> FamilyRole { get; set; }
        public virtual ICollection<NeighborRequest> NeighborRequest { get; set; }
        public virtual ICollection<Sharing> Sharing { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Story> Story { get; set; }
    }

    public class FamilyRole
    {
        public int userId { get; set; }
        public int familyId { get; set; }
        public string roleName { get; set; }
        public System.DateTime dateJoin { get; set; }

        public virtual Family Family { get; set; }
        public virtual User User { get; set; }
    }

    public class Neighborhood
    {
        public int familyId { get; set; }
        public int neighborId { get; set; }
        public System.DateTime date { get; set; }

        public virtual Family Family { get; set; }
        public virtual Family Neighbor { get; set; }
    }
    public class NeighborRequest
    {
        public int userId { get; set; }
        public int familyId { get; set; }
        public string status { get; set; }

        public virtual Family Family { get; set; }
        public virtual User User { get; set; }
    }


}