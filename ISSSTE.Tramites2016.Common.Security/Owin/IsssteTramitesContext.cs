namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using ISSSTE.Tramites2016.Common.Security.Core;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Provider;
    using Claims;
    using Newtonsoft.Json;

    /// <summary>
    /// Contiene toda la información que es utilizada en el proceso de autenticación por el sistema de seguridad del ISSSTE
    /// </summary>
    internal class IsssteTramitesContext : BaseContext
    {
        #region Constants

        private const string StringClaimDataType = "http://www.w3.org/2001/XMLSchema#string";

        #endregion

        #region Properties

        /// <summary>
        /// Obtiene la opciones de autenticación
        /// </summary>
        public IsssteTramitesAuthenticationOptions AuthenticationOptions { get; private set; }

        /// <summary>
        /// Obtiene las propiedades de autenticación
        /// </summary>
        public AuthenticationProperties AuthenticationProperties { get; private set; }

        /// <summary>
        /// Ontiene la identidad autenticada
        /// </summary>
        public ClaimsIdentity AuthenticatedIdentity
        {
            get
            {
                var identity = new ClaimsIdentity(this.AuthenticationOptions.SignInAsAuthenticationType);

                identity.AddClaims(this.GetUserClaims(this.User, this.AccessToken));

                return identity;
            }
        }

        /// <summary>
        /// Obtiene la información del usuario
        /// </summary>
        public IsssteUser User { get; private set; }
        
        /// <summary>
        /// Obtiene el token de acceso
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Obtiene el refresh token
        /// </summary>
        public string RefreshToken { get; private set; }

        #endregion

        #region Constructo

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context"></param>
        /// <param name="authenticationOptions"></param>
        /// <param name="state"></param>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        public IsssteTramitesContext(IOwinContext context, IsssteTramitesAuthenticationOptions authenticationOptions, string state, IsssteUser user, string accessToken, string refreshToken)
            : base(context)
        {
            this.AuthenticationOptions = authenticationOptions;
            this.AuthenticationProperties = this.AuthenticationOptions.StateDataFormat.Unprotect(state);
            this.User = user;
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;

            this.AuthenticationProperties.Dictionary.Add("ExternalAccessToken", this.AccessToken);
            this.AuthenticationProperties.Dictionary.Add("ExternalRefreshToken", this.RefreshToken);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Genera un listado de claim con la información de usuario y el token de acceso
        /// </summary>
        /// <param name="user">Información del usuario del cual obtener los claims</param>
        /// <param name="accessToken">El token de acceso el cual incluir en los cliams</param>
        /// <returns>Listado de claims</returns>
        internal List<Claim> GetUserClaims(IsssteUser user, string accessToken)
        {
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id, StringClaimDataType),
                new Claim(ClaimTypes.Name, user.UserName, StringClaimDataType),
                new Claim(ClaimTypes.GivenName, user.Name, StringClaimDataType),
                new Claim(ClaimTypes.Email, user.Email, StringClaimDataType),
                new Claim(IsssteTramitesClaimTypes.Properties, JsonConvert.SerializeObject(user.Properties), IsssteTramitesClaimDataTypes.Properties),
                new Claim(IsssteTramitesClaimTypes.Roles, JsonConvert.SerializeObject(user.Roles), IsssteTramitesClaimDataTypes.Roles),
                new Claim(IsssteTramitesClaimTypes.AccessToken, accessToken, StringClaimDataType)
            };

            // claimList.Add(new Claim(IsssteTramitesClaimTypes.AccessToken, this.AccessToken, StringClaimDataType));
            return claimList;
        }

        #endregion
    }
}