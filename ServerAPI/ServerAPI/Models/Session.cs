using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famsam.serverapi.Models
{
    public class Session
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}