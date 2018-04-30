using System.Web.Mvc;
using System.Web.Routing;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection Routes)
        {
            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Routes.MapMvcAttributeRoutes();

            Routes.MapRoute(
                "Administrador",
                "Administrador",
                new {controller = "Administrador", action = "Index"}
            );

            Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Entitle", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}