using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;


namespace ISSSTE.Tramites2016.Common.Security.Web
{

    /// <summary>
    /// Representa el resultado de una solicitud de autenticación al sistema de seguridad del ISSSTE
    /// </summary>
    public class IsssteChallengeResult : HttpUnauthorizedResult
    {
        #region Fields

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        #endregion

        #region Properties

        /// <summary>
        /// Obtiene o asigna el proveedor de inicio de sesión
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Obtiene o asigna la url a la cual regresar la respuesta
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Obtiene o asigna el Id del usuario
        /// </summary>
        public string UserId { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="provider">Proveedor de incio de sesión</param>
        /// <param name="redirectUri">Url a la que enviar la respuesta</param>
        public IsssteChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="provider">Proveedor de incio de sesión</param>
        /// <param name="redirectUri">Url a la que enviar la respuesta</param>
        /// <param name="userId">Id del usuario</param>
        public IsssteChallengeResult(string provider, string redirectUri, string userId)
        {
            this.LoginProvider = provider;
            this.RedirectUri = redirectUri;
            this.UserId = userId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ejecuta el resultado
        /// </summary>
        /// <param name="context">Contexto del controlador</param>
        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
            if (this.UserId != null)
            {
                properties.Dictionary[XsrfKey] = this.UserId;
            }

            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
        }

        #endregion
    }
}