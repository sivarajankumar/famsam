using ServerAPI.App_Start;
using ServerAPI.CF_Models;
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
                UserRole userRole = new UserRole();
                userRole.rolename = UserRole.LOGGED_IN_ROLE;
                User createUser = new User
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
                
                //new post
                DateTime thisTime = DateTime.Now;
                GeneralPost post = new GeneralPost();
                post.Id = thisTime.Millisecond;
                post.lastUpdate = thisTime;
                post.description = "haha";
                post.CreateUser = createUser;
                post.createUserId = createUser.id;
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
