using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Core;
using ISSSTE.Tramites2016.Common.Security.Owin;
using ISSSTE.Tramites2016.Common.Web;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Security.ServiceAgents.Implementation
{
    /// <summary>
    /// Service agent utilizado para la obtención de información del usuario que se acaba de loguear
    /// </summary>
    internal class SecurityAuthenticationServiceAgent : BaseSecurityServiceAgent
    {
        #region Properties

        /// <summary>
        /// Plantilla de la url relativa donde obtener la información de un usuario
        /// </summary>
        private const string UserInfoApiRelativePathTemplate = "api/Procedures/{0}/Users/{1}";
        /// <summary>
        /// Cadena que representa como solicitar un nuevo bearer token utilizando un refresh token
        /// </summary>
        private const string RefreshTokenDataTemplate = "grant_type=refresh_token&refresh_token={0}&client_id={1}";

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene la información del usuario dentro del trámite enviado
        /// </summary>
        /// <param name="accessToken">OAuth 2.0 bearer token a utilizar</param>
        /// <param name="procedureId">Id del trámite</param>
        /// <param name="userId">Id del usuario</param>
        /// <returns>Información del usuario dentro del trámite</returns>
        internal async Task<IsssteUser> GetUserInfoAsync(string accessToken, string procedureId, string userId)
        {
            var address = IsssteTramitesConstants.BaseUrl + String.Format(UserInfoApiRelativePathTemplate, procedureId, userId);

            var httpClient = BuildHttpClient(address, accessToken);

            var response = await httpClient.GetAsync(address);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<IsssteUser>(json);

            return user;
        }

        /// <summary>
        /// Obtiene un nuevo OAuth 2.0 bearer token para el usuario logueado utilizando su refresh token
        /// </summary>
        /// <param name="owinContext">Contexto Owin a utilizar</param>
        /// <param name="clientId">Id del cliente con que cual realizar la petición</param>
        /// <returns>Respuesta de la petición del token</returns>
        internal async Task<IsssteOAuthTokenResponse> GetAuthTokenFromRefreshTokenAsync(IOwinContext owinContext, string clientId)
        {
            var refreshToken = base.GetUserRefreshToken<IsssteIdentityUser>(owinContext);

            var address = IsssteTramitesConstants.TokenUrl;

            var httpClient = BuildHttpClient(address);

            var content = new StringContent(String.Format(RefreshTokenDataTemplate, refreshToken, clientId));
            content.Headers.ContentType = new MediaTypeHeaderValue(HttpContants.ContentTypes.FormUrlEncode);

            var response = await httpClient.PostAsync(IsssteTramitesConstants.TokenUrl, content);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IsssteOAuthTokenResponse>(json);

            return result;
        }

        /// <summary>
        /// Válida que el bearer token del usuario continue siendo válido
        /// </summary>
        /// <param name="owinContext">Contexto Owin a utilizar</param>
        /// <returns>Resultado de la validación</returns>
        internal async Task ValidateAccessToken(IOwinContext owinContext)
        {
            var token = base.GetUserAccessToken<IsssteIdentityUser>(owinContext);

            var baseAddress = IsssteTramitesConstants.TokenValidationUrl;

            var http = BuildHttpClient(baseAddress, token);

            var response = await http.GetAsync(baseAddress);

            response.EnsureSuccessStatusCode();
        }

        #endregion
    }
}
