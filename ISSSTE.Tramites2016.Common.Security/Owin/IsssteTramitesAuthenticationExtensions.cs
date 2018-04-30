using Owin;

namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    /// <summary>
    /// Contiene métodos para autorizar la aplicación mediante el sistema de seguridad del ISSSTE
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "OWN necesita tener el scope superior.")]
    public static class IsssteTramitesAuthenticationExtensions
    {
        /// <summary>
        /// Agrega al pipeline de procesamiento de Owin la utenticación mediante el sistema de seguridad del ISSSTE
        /// </summary>
        /// <param name="app">Applicación Owin</param>
        /// <param name="options">Opciones de autenticación</param>
        /// <returns>Aplicación Owin</returns>
        public static IAppBuilder UseIsssteTramitesAuthentication(this IAppBuilder app, IsssteTramitesAuthenticationOptions options)
        {
            return app.Use(typeof(IsssteTramitesAuthenticationMiddleware), app, options);
        }

        /// <summary>
        /// Agrega al pipeline de procesamiento de Owin la utenticación mediante el sistema de seguridad del ISSSTE
        /// </summary>
        /// <param name="app">Applicación Owin a utilizar</param>
        /// <param name="procedureId">Id del trámite a utilizar</param>
        /// <param name="clientId">Id del cliente a utilizar</param>
        /// <param name="secret">Secreto a utilizar</param>
        /// <param name="errorUrl">Url a la que redirigir en caso de error</param>
        /// <returns>Aplicación Owin</returns>
        public static IAppBuilder UseIsssteTramitesAuthentication(this IAppBuilder app, string procedureId, string clientId, string secret, string errorUrl)
        {
            return app.Use(typeof(IsssteTramitesAuthenticationMiddleware), app, new IsssteTramitesAuthenticationOptions(procedureId, clientId, secret, errorUrl));
        }
    }
}