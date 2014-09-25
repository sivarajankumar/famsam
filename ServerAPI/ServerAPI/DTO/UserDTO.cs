using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.Models
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResultContentDTO
    {
        public string Message { get; set; }
        public string Token { get; set; }
    }

    public class UserErrorResult
    {
        public static ApiResult LOGIN_FAIL = new ApiResult { Code = "001", Content = "login fail" };
        public static ApiResult EMAIL_NOT_EXIST = new ApiResult { Code = "002", Content = "email not exist" };
        public static ApiResult REGISTER_FAIL = new ApiResult { Code = "0022", Content = "register fail" };
    }
}