using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.DTO
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

    public class UserApiResult
    {
        public static ApiResult LoginFail { get; private set; }
        public static ApiResult EmailNotExist { get; private set; }
        public static ApiResult RegisterFail { get; private set; }
        public static ApiResult LoginSuccess { get; private set; }
        public static ApiResult RegisterSuccess { get; private set; }

        static UserApiResult(){
            LoginFail = new ApiResult("001", "login fail");
            EmailNotExist = new ApiResult("002", "email not exist");
            RegisterFail = new ApiResult("0022", "register fail");
            LoginSuccess = new ApiResult("0012", "login success");
            RegisterSuccess = new ApiResult("003", "regiseter success");
        }
    }
}