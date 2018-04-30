using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration Config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            Config.SuppressDefaultHostAuthentication();
            Config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Rutas de API web
            Config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}