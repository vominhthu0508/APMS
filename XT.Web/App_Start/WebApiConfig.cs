using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace XT.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove Xml formatters. This means when we visit an endpoint from a browser,
            // Instead of returning Xml, it will return Json.
            // More information from Dave Ward: http://jpapa.me/P4vdx6
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            // Configure json camelCasing per the following post: http://jpapa.me/NqC2HH
            // Here we configure it to write JSON property names with camel casing
            // without changing our server-side data model:
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //add string enum converter with camel case text.
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }
    }
}
