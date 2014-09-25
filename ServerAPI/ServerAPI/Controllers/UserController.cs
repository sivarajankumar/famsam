using famsam.serverapi.Models;
using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServerAPI.Controllers
{
    public class UserController : ApiController
    {
        private FamsamDB db = new FamsamDB();
        public IHttpActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            ApiResult result = new ApiResult();

            IQueryable<User> query = from a in db.User
                                     where (a.Email == userLogin.Email && a.Password == userLogin.Password)
                                            select a;
            User user = query.First<User>();
            if (user == null)
            {
                return Ok(UserErrorResult.LOGIN_FAIL);
            }
            else
            {
                //login success
                Session session = new Session();
                session.User = user;
                DateTime now = DateTime.Now;
                session.Token = now.Millisecond + "";
                session.ExpiredDate = now.AddHours(2);
                db.Session.Add(session);

                LoginResultContentDTO loginDTO = new LoginResultContentDTO();
                loginDTO.Message = "login success";
                loginDTO.Token = session.Token;

                result.Code = "002";
                result.Content = loginDTO;

                return Ok(result);
            }
            
        }

        public IHttpActionResult Register([FromBody] User user)
        {
            //check exist email
            IQueryable<User> query = from u in db.User 
                                     where (u.Email == user.Email)
                                     select u;
            User existUser = query.First<User>();
            if (existUser == null)
            {
                //add user to db
                db.User.Add(user);
                //result
                return Ok(new ApiResult {Code = "003", Content = "register success"});
            }
            else
            {
                //reg fail
                return Ok(UserErrorResult.REGISTER_FAIL);
            }
            
        }
    }

}
