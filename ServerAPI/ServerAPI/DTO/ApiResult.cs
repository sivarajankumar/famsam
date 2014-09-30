using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.DTO
{
    public class ApiResult
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public dynamic Content { get; set; }

        public ApiResult(string Code, string Message)
        {
            this.Code = Code;
            this.Message = Message;
        }

        public ApiResult(string Code, string Message, dynamic Content)
        {
            this.Code = Code;
            this.Message = Message;
            this.Content = Content;
        }

        public ApiResult clone()
        {
            ApiResult r = new ApiResult(this.Code, this.Message);
            r.Content = this.Content;
            return r;
        }
    }
}