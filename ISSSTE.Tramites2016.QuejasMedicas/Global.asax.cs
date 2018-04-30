using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            UnityApiConfig.RegisterComponents(); //MFP  Registro de componentes

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Establecer el tipo de Claim que es utilizado  
            // para identificar al usuario de forma única.     
            //AntiForgeryConfig.UniqueClaimTypeIdentifier =
            //    ClaimTypes.NameIdentifier; MFP 23-06-2017

            //Configura comportamiento de la serialización JSON de Web API
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "dd/MM/yyyy";
        }
    }
}