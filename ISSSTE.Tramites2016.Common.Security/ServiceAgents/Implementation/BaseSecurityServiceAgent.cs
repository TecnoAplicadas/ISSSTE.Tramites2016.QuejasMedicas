using ISSSTE.Tramites2016.Common.Security.Claims;
using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Resources;
using ISSSTE.Tramites2016.Common.Web;
using Microsoft.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ISSSTE.Tramites2016.Common.Security.ServiceAgents.Implementation
{
    /// <summary>
    /// Clase base para los ServiceAgents que consumen servicios del sistema de seguridad del ISSSTE
    /// </summary>
    public abstract class BaseSecurityServiceAgent
    {
        #region Internal Methods

        /// <summary>
        /// Genera un cliente http configurado con un una dirección destino y un token Oauth.
        /// </summary>
        /// <param name="address">Direccion de la peticion</param>
        /// <param name="accessToken">Token a usar</param>
        /// <returns>La respuesta del servicio</returns>
        internal HttpClient BuildHttpClient(string address, string accessToken)
        {
            var client = this.BuildHttpClient(address);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(HttpContants.Headers.Authorization.Values.Bearer, accessToken);

            return client;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Genera un cliente http configurado con un una dirección destino.
        /// </summary>
        /// <param name="address">Direccion de la peticion</param>
        /// <returns>La respuesta del servicio</returns>
        protected HttpClient BuildHttpClient(string address)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContants.ContentTypes.Json));

            return client;
        }

        /// <summary>
        /// Obtiene el token de acceso al sistema de seguridad que posee el usuario logueado
        /// </summary>
        /// <typeparam name="TUser">Tipo de usuario</typeparam>
        /// <param name="owinContext">Contexto Owin a utilizar</param>
        /// <returns>OAuth 2.0 bearer token del sistema de seguridad</returns>
        protected string GetUserAccessToken<TUser>(IOwinContext owinContext)
            where TUser : IsssteIdentityUser, new()
        {
            string token = null;

            if (owinContext.Authentication.User.Identity.IsAuthenticated)
            {
                var tokenClaim = owinContext.Authentication.User.Claims
                    .Where(c => c.Type == IsssteTramitesClaimTypes.AccessToken)
                    .FirstOrDefault();

                if (tokenClaim != null)
                {
                    token = tokenClaim.Value;
                }
            }

            if (token == null)
                throw new InvalidOperationException(ErrorMessages.GetBearerTokenFailed);

            return token;
        }

        /// <summary>
        /// Obtiene el refresh token de acceso al sistema de seguridad que posee el usuario logueado
        /// </summary>
        /// <typeparam name="TUser">Tipo de usuario</typeparam>
        /// <param name="owinContext">Contexto Owin a utilizar</param>
        /// <returns>OAuth 2.0 refresh token del sistema de seguridad</returns>
        protected string GetUserRefreshToken<TUser>(IOwinContext owinContext)
            where TUser : IsssteIdentityUser, new()
        {
            string refreshToken = null;

            if (owinContext.Authentication.User.Identity.IsAuthenticated)
            {
                var refreshTokenClaim = owinContext.Authentication.User.Claims
                    .Where(c => c.Type == IsssteTramitesClaimTypes.RefreshToken)
                    .FirstOrDefault();

                if (refreshTokenClaim != null)
                {
                    refreshToken = refreshTokenClaim.Value;
                }
            }

            if (refreshToken == null)
                throw new InvalidOperationException(ErrorMessages.GetRefreshToketFailed);

            return refreshToken;
        }

        #endregion
    }
}
