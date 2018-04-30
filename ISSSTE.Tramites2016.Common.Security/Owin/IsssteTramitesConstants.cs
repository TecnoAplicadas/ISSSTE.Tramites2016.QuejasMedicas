using System.Configuration;

namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    /// <summary>
    ///     Contiene constantes a utilizar durante le proceso de autorización
    /// </summary>
    public class IsssteTramitesConstants
    {
        #region Fields

        /// <summary>
        ///     Tipo de autenticación para el sistema de seguridad del ISSSTE del app.config
        /// </summary>
        public const string DefaultAuthenticationType = "Tramites ISSSTE";

        #endregion

        #region Properties

        /// <summary>
        ///     Url base del sistema de seguridad del ISSSTE del app.config
        /// </summary>
        public static string BaseUrl => ConfigurationManager.AppSettings[
            "ISSSTE.Tramites2016.Common.Security.Owin.Constants.Server.WSBaseUrl"];

        /// <summary>
        ///     Url absoluta para obtener un token OAuth 2.0 dentro del sistema de seguridad del ISSSTE del app.config
        /// </summary>
        public static string TokenUrl => ConfigurationManager.AppSettings[
            "ISSSTE.Tramites2016.Common.Security.Owin.Constants.Client.TokenUrl"];

        /// <summary>
        ///     Url absoluta para validar un token OAuth 2.0 dentro del sistema de seguridad del ISSSTE del app.config
        /// </summary>
        public static string TokenValidationUrl => ConfigurationManager.AppSettings[
            "ISSSTE.Tramites2016.Common.Security.Owin.Constants.Server.TokenValidationUrl"];

        /// <summary>
        ///     Url absoluta para iniciar sesión dentro del sistema de seguridad del ISSSTE del app.config
        /// </summary>
        public static string LogoutUrl => ConfigurationManager.AppSettings[
            "ISSSTE.Tramites2016.Common.Security.Owin.Constants.Client.LogoutUrl"];

        /// <summary>
        ///     Url absoluta para cerrar sesión dentro del sistema de seguridad del ISSSTE del app.config
        /// </summary>
        public static string LoginUrl => ConfigurationManager.AppSettings[
            "ISSSTE.Tramites2016.Common.Security.Owin.Constants.Client.LoginUrl"];

        #endregion
    }
}