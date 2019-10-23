using System.Linq;
using System.Web.Http;
using PNMS.Web.API.Handlers;

namespace PNMS.Web.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // Web API configuration and services
            // Performs JWT Identity Validation
            config.MessageHandlers.Add(new AuthenticationHandler());
        }
    }
}
