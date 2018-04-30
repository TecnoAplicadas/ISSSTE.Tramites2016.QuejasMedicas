using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.Util;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public class UnityConfig
    {
        /// <summary>
        /// Obtiene la ruta relativa donde se encuentra el html a utilizar como plantilla de los correos
        /// </summary>
        private static string MailMasterPagePath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["MailMasterPagePath"]);
            }
        }

        /// <summary>
        /// Obtiene la ruta relativa donde se encuentra la a utilizar como logo para los correos
        /// </summary>
        private static string MailMasterPageLogoPath
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["MailMasterPageLogoPath"]);
            }
        }

        //public static void RegisterComponents()
        //{
        //var container = new UnityContainer();Comentado MFP 27/01/2017

        // register all your components with the container here
        // it is NOT necessary to register your controllers
        // e.g. container.RegisterType<ITestService, TestService>();

        //Comentado MFP 27/01/2017
        //GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container); 
        //}

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        ///     There is no need to register concrete types such as controllers or API controllers (unless you want to
        ///     change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            //Register your types here
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IMailService, MailService>(new InjectionConstructor(MailMasterPagePath, MailMasterPageLogoPath));
        }


        /// <summary>
        ///     Agrego MFP 27/01/2017
        /// </summary>
        private static readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        ///     Agrego MFP 27/01/2017
        ///     Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

    }
}