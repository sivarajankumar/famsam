using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.Models
{
    public class ApiResult
    {
        public string Code { get; set; }
        public dynamic Content { get; set; }
    }
}