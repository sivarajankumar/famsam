
//using ServerAPI.Models;
using ServerAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServerAPI.CF_Models;

namespace ServerAPI.Controllers
{
    public class UserController : ApiController
    {
        private CF_FamsamEntities db = new CF_FamsamEntities();
        [HttpPost]
        [ActionName("login")]
        public IHttpActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            ApiResult result;
            
            IQueryable<User> query = from a in db.User
                                     where (a.email == userLogin.Email && a.password == userLogin.Password)
                                            select a;
            User user = query.FirstOrDefault<User>();
            if (user == null)
            {
                return Ok(UserApiResult.LoginFail);
            }
            else
            {
                //login success
                Session session = new Session();
                session.User = user;
                DateTime now = DateTime.Now;
                session.token = now.Millisecond + "";
                session.expired = now.AddHours(2);
                db.Session.Add(session);
                db.SaveChanges();

                LoginResultContentDTO loginDTO = new LoginResultContentDTO();
                loginDTO.Message = "login success";
                loginDTO.Token = session.token;

                result = UserApiResult.LoginSuccess;
                result.Content = loginDTO;

                return Ok(result);
            }
            
        }

        [HttpPost]
        [ActionName("register")]
        public IHttpActionResult Register([FromBody] User user)
        {
            //check exist email
            IQueryable<User> query = from u in db.User 
                                     where (u.email == user.email)
                                     select u;
            User existUser = query.FirstOrDefault<User>();
            if (existUser == null)
            {
                //add user to db
                db.User.Add(user);
                db.SaveChanges();
                //result
                return Ok();
            }
            else
            {
                //reg fail
                return Ok();
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
