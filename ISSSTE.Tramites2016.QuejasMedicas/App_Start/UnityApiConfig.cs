using System.Configuration;
using System.Web.Hosting;
using System.Web.Http;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.Util;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    /// <summary>
    ///     Agrego MFP 27/01/2017
    /// </summary>
    public static class UnityApiConfig
    {
        #region Static Methods

        /// <summary>
        ///     Obtiene la ruta relativa donde se encuentra el html a utilizar como plantilla de los correos
        /// </summary>
        private static string MailMasterPagePath => HostingEnvironment.MapPath(
            ConfigurationManager.AppSettings["MailMasterPagePath"]);

        /// <summary>
        ///     Obtiene la ruta relativa donde se encuentra la a utilizar como logo para los correos
        /// </summary>
        private static string MailMasterPageLogoPath => HostingEnvironment.MapPath(
            ConfigurationManager.AppSettings["MailMasterPageLogoPath"]);

        /// <summary>
        ///     Registra los tipos necesarios para los controladores Web API
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ILogger, Logger>();
            container.RegisterType<ICalendarDomainService, CalendarDomainService>();
            container.RegisterType<ICommonDomainService, CommonDomainService>();
            container.RegisterType<ISolcitcitudService, RequestDomainService>();
            container.RegisterType<IConfiguracionesService, ConfiguracionesService>();
            container.RegisterType<IMailService, MailService>(
                new InjectionConstructor(MailMasterPagePath, MailMasterPageLogoPath));


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        #endregion
    }
}