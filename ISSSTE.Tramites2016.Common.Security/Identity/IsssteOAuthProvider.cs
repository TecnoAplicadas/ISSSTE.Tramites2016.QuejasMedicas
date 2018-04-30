using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ISSSTE.Tramites2016.Common.Security.ServiceAgents.Implementation;

namespace ISSSTE.Tramites2016.Common.Security.Identity
{
    /// <summary>
    /// Proveedor de autenticación para ASP Identity que utiliza OAuth 2.0
    /// </summary>
    /// <typeparam name="TUser">Tipo del usuario</typeparam>
    public class IsssteOAuthProvider<TUser> : OAuthAuthorizationServerProvider
        where TUser : IsssteIdentityUser, new()
    {
        #region Fields
    
        /// <summary>
        /// Id de cliente a utilizar
        /// </summary>
        private readonly string _clientId;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="clientId">Id de cliente a utilizar</param>
        public IsssteOAuthProvider(string clientId)
        {
            if (clientId == null)
                throw new ArgumentNullException("publicClientId");

            _clientId = clientId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Procesa la solicitud de un nuevo Token OAuth 2.0 mediante password
        /// </summary>
        /// <param name="context">Contexto de la petición</param>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                using (var userManager = context.OwinContext.GetUserManager<IsssteUserManager<TUser>>())
                {
                    var user = await userManager.FindByNameAsync(context.UserName);

                    if (user == null)
                    {
                        context.SetError("invalid_grant", "The user name is incorrect.");
                        return;
                    }
                    else
                    {

                        try
                        {
                            await new SecurityAuthenticationServiceAgent().ValidateAccessToken(context.OwinContext);
                        }
                        catch (HttpRequestException e)
                        {
                            Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                            context.SetError("invalid_access_token", "The access token is invalid.");
                            return;
                        }
                        catch(Exception e)
                        {
                            Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                            context.SetError("validation_error", "There was a problem trying to validate the token.");
                            return;
                        }
                    }

                    /*Validación a partir de Refresh Token*/
                    //if (context.OwinContext.Authentication.User == null && user.Claims.Any(c => c.ClaimType == IsssteTramitesClaimTypes.RefreshToken))
                    //{
                    //    var refreshTokenCliam = user.Claims.First(c => c.ClaimType == IsssteTramitesClaimTypes.RefreshToken);

                    //    var helper = new IsssteAuthenticationHelper();

                    //    IsssteOAuthTokenResponse tokenResponse = null;

                    //    try
                    //    {
                    //        tokenResponse =
                    //            await
                    //                helper.GetAuthTokenFromRefreshTokenAsync(refreshTokenCliam.ClaimValue, context.ClientId);
                    //    }
                    //    catch (HttpRequestException e)
                    //    {
                    //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    //        context.SetError("invalid_refresh_token", "The refresh token is invalid.");
                    //        return;
                    //    }

                    //    try
                    //    {
                    //        var userInfo = await helper.GetUserInfoAsync(tokenResponse.Access_Token, Startup.ProcedureId, tokenResponse.User_Id);

                    //        var userClaims = user.Claims.ToList();

                    //        userClaims.ForEach(c => userManager.RemoveClaim(user.Id, new Claim(c.ClaimType, c.ClaimValue)));

                    //        var newUserClaims = IsssteAuthenticationHelper.GetUserClaims(userInfo.Item1, userInfo.Item2, userInfo.Item3, tokenResponse.Access_Token);

                    //        newUserClaims.ForEach(c => userManager.AddClaim(user.Id, c));
                    //    }
                    //    catch (HttpRequestException e)
                    //    {
                    //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    //        context.SetError("error_user_info", "There was an error obtaining the user information");
                    //        return;
                    //    }
                    //}

                    ClaimsIdentity oAuthIdentity =
                        await userManager.GenerateUserIdentityAsync(user, OAuthDefaults.AuthenticationType);
                    ClaimsIdentity cookiesIdentity =
                        await userManager.GenerateUserIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);

                    AuthenticationProperties properties = CreateProperties(user.Id, user.UserName, user.Email, context.ClientId);
                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        /// <summary>
        /// Se llama tras una autenticación correcta y se utiliza para agregar propiedades adicionales a la respueata del token
        /// </summary>
        /// <param name="context">Contexto de la petición</param>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Se llama para validar que se haya enviado un cliente válido en la petición del token
        /// </summary>
        /// <param name="context">Contexto de la petición</param>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                //context.Validated();
                //return Task.FromResult<object>(null);

                context.Rejected();
                context.SetError("invalid_clientId", "ClientId should be sent.");
                return;
            }

            context.Validated();
        }


        /// <summary>
        /// Se llama para validar que la url de respuesta sea un url válida para el cliente
        /// </summary>
        /// <param name="context">Contexto de la petición</param>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _clientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Crea las propiedades de autenticación del usuario que intenta loguearse
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <param name="userName">Nombre de usuario del usuario</param>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <param name="clientId">Id del cliente que solicita la autenticación</param>
        /// <returns>Propiedades de autenticación</returns>
        public static AuthenticationProperties CreateProperties(string userId, string userName, string email, string clientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "user_id", userId },
                { "user_name", userName },
                { "email", email }
            };
            return new AuthenticationProperties(data);
        }

        #endregion
    }
}