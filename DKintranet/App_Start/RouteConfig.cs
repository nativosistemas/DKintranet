using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DKintranet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "config", action = "LoginIntranet", id = UrlParameter.Optional }
            );
            ////routes.MapPageRoute("Default",
            ////   "home/{action}",
            ////   "~/index.aspx");

        }
    }
}
