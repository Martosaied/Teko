using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Teko.Web.Config
{
    public class SeoFriendlyRoute : Route
    {
        public SeoFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("id"))
                    routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
            }

            return routeData;
        }

        private object GetIdValue(object id)
        {
            if (id != null)
            {
                string idValue = id.ToString();

                var regex = new Regex(@"^(?<id>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["id"].Value;
                }
            }

            return id;
        }
    }
    public class SeoFriendlyRouteVerTodo : Route
    {
        public SeoFriendlyRouteVerTodo(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("title"))
                    routeData.Values["title"] = GetIdValue(routeData.Values["title"]);
            }

            return routeData;
        }

        private object GetIdValue(object title)
        {
            if (title != null)
            {
                string idValue = title.ToString();

                var regex = new Regex(@"^(?<title>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["title"].Value;
                }
            }

            return title;
        }
    }
    public class SeoFriendlyRouteTag : Route
    {
        public SeoFriendlyRouteTag(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("Tag"))
                    routeData.Values["Tag"] = GetIdValue(routeData.Values["Tag"]);
            }

            return routeData;
        }

        private object GetIdValue(object Tag)
        {
            if (Tag != null)
            {
                string tag = Tag.ToString();
                tag = tag.Replace("-", " ");
                return tag;
            }

            return Tag;
        }
    }
}