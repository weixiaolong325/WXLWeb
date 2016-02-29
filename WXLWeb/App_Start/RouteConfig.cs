using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WXLWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            
            //默认路由规则
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //新增 id.html结尾路由
            routes.MapRoute(
                "IDHtml",
                "{controller}/{action}/{id}.html",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}