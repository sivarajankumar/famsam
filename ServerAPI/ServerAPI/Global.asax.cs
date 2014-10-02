using ServerAPI.App_Start;
using ServerAPI.CF_Models;
using ServerAPI.Utils;
//using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ServerAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CF_FamsamEntities>());
            InitializeDB();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void InitializeDB()
        {
            using (CF_FamsamEntities context = new CF_FamsamEntities())
            {
                //create user role
                UserRole userRole = context.UserRole.Find(UserRole.LOGGED_IN_ROLE);
                if ( userRole == null)
                {
                    userRole = new UserRole();
                    userRole.rolename = UserRole.LOGGED_IN_ROLE;
                    context.UserRole.Add(userRole);
                }
                
                //create user
                User createUser = (from u in context.User where u.email == "mrbean" select u).FirstOrDefault<User>();
                if (createUser == null)
                {
                    createUser = new User
                    {
                        id = DateTime.Now.Millisecond,
                        email = "mrbean@xyz.com",
                        password = "mrbean",
                        firstname = "Lup",
                        lastname = "Bean",
                        UserRole = userRole,
                        role = userRole.rolename
                    };
                    context.User.Add(createUser);
                }
                
                //create session
                string token = Base64Utils.Base64Encode("mrbean:mrbean");
                Session session = context.Session.Find(token);
                if (session == null)
                {
                    session = new Session
                    {
                        token = token,
                        expired = new DateTime(2100, 1, 1),
                        User = createUser,
                    };
                    context.Session.Add(session);
                }
                
                //new post for photo
                DateTime thisTime = DateTime.Now;
                GeneralPost post = new GeneralPost();
                post.Id = thisTime.Millisecond;
                post.lastUpdate = thisTime;
                post.description = "haha";
                post.CreateUser = createUser;
                post.createUserId = createUser.id;
                post.postType = GeneralPost.PHOTO_POST_TYPE;
                context.GeneralPost.Add(post);
                
                Photo photo = new Photo();
                photo.Post = post;
                photo.url = "http://photo.url/nothing.jpg";
                context.Photo.Add(photo);
                try 
                { 
                    context.SaveChanges(); 
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception on Initialize DB Sample: " + ex);
                }
            }

        }

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    var route = routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "api/{controller}/{id}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );
        //    route.RouteHandler = new MyHttpControllerRouteHandler();
        //}

        
    }
}
