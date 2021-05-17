using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ScanerApi.Areas.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Web_default",
               "Web/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
