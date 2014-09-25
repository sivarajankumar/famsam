using ServerAPI.App_Start;
using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Database.SetInitializer(new DropCreateDatabaseAlways<FamsamDB>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
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
