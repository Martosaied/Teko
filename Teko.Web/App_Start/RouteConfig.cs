using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Teko.Web.Config;

namespace Teko.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("ContenidoDetails", new SeoFriendlyRoute("contenidos/detalles/{id}",
            new RouteValueDictionary(new { controller = "Contenidos", action = "Detalles" }),
            new MvcRouteHandler()));

            routes.Add("ContenidoSearch", new SeoFriendlyRoute("contenidos/buscar/{Buscador}",
            new RouteValueDictionary(new { controller = "Contenidos", action = "Buscar" }),
            new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Teko.Controllers" }
            );




        }
    }
}
