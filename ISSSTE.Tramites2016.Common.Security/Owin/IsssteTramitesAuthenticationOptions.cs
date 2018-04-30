namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    using System;
    using System.Net.Http;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    /// <summary>
    /// Representa las información adicional necesaria para utilizar el sistema de seguridad del ISSSTE como proveedor de autenticación Owin
    /// </summary>
    public class IsssteTramitesAuthenticationOptions : AuthenticationOptions
    {
        #region Constants

        /// <summary>
        /// Url relativa a la que se escuchara para interpretar la respuesta del sistema de seguridad del ISSSTE
        /// </summary>
        private const string CallBackUrl = "/signin-issstetramite";

        #endregion

        #region Properties

        /// <summary>
        /// Obtiene o asigna la url a la cual se debe de enviar la respueta del sistema de seguridad del ISSSTE
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// Obtiene o asigna el tipo de autenticación
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        /// Obtiene o asigna el estado de la solicitud
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <summary>
        /// Obtiene o asigna el timeout para el canal de comunicación
        /// </summary>
        public TimeSpan BackchannelTimeout { get; set; }

        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        /// Obtiene o asigna el Id del trámite a utilizar
        /// </summary>
        public string ProcedureId { get; set; }

        /// <summary>
        /// Obtiene o asigna el Id de cliente a utilizar
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Obtiene o asigna el secreto a utilizar
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Obtiene o asigna la url a la qcual redirigir si ocurriese un error
        /// </summary>
        public string ErrorUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="procedureId">Id del trámite a utilizar</param>
        /// <param name="clientId">Id del cliente a utilizar</param>
        /// <param name="secret">Secreto a utilizar</param>
        /// <param name="errorUrl">Url a la que redirigir en caso de error</param>
        public IsssteTramitesAuthenticationOptions(string procedureId, string clientId, string secret, string errorUrl)
            : base(IsssteTramitesConstants.DefaultAuthenticationType)
        {
            this.Description.Caption = IsssteTramitesConstants.DefaultAuthenticationType;
            this.CallbackPath = new PathString(CallBackUrl);
            this.AuthenticationMode = AuthenticationMode.Passive;
            this.ProcedureId = procedureId;
            this.ClientId = clientId;
            this.Secret = secret;
            this.ErrorUrl = errorUrl;
            this.BackchannelTimeout = TimeSpan.FromSeconds(60.0);
        }

        #endregion
    }
}