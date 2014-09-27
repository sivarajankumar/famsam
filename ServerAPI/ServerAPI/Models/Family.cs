using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace famsam.serverapi.Models
{
    public class Family
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string CoverURL { get; set; }
        public string Description { get; set; }
    }

    public class NeighborHood
    {
        [Key, ForeignKey("Family"), Column(Order = 0)]
        public long FamilyId { get; set; }
        [Key, ForeignKey("Family"), Column(Order = 1)]
        public long NeighborId { get; set; }

        public Family Family { get; set; }
        public Family Neighbor { get; set; }
        public DateTime Date { get; set; }
    }

    public class FamilyRole
    {
        [Key, ForeignKey("Family"), Column(Order = 0)]
        public long FamilyId { get; set; }
        [Key, ForeignKey("User"), Column(Order = 1)]
        public long UserId { get; set; }

        public Family Family { get; set; }
        public User User { get; set; }
        public bool isManager { get; set; }
        public DateTime DateJoin { get; set; }
    }

    public class NeighborRequest
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public long UserId { get; set; }
        [Key, ForeignKey("Family"), Column(Order = 1)]
        public long FamilyId { get; set; }

        public User User { get; set; }
        public Family Family { get; set; }
        public string Status { get; set; }
    }
}