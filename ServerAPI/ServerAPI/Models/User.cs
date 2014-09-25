using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace famsam.serverapi.Models
{
    
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string About { get; set; }
        public string WorkAt { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string InfoPrivacy { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public UserRole UserRole { get; set; }
        public List<Family> Families { get; set; }
    }

    public class UserRole
    {
        [Key]
        public long RoleName { get; set; }
        public string Description { get; set; }
    }

}