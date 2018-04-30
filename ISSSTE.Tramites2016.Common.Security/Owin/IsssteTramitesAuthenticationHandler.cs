namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Logging;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;
    using Newtonsoft.Json;
    using ServiceAgents.Implementation;

    /// <summary>
    /// Manejador del proceso de autenticación con el sistema de seguridad del ISSSTE
    /// </summary>
    internal class IsssteTramitesAuthenticationHandler : AuthenticationHandler<IsssteTramitesAuthenticationOptions>
    {
        #region Fields

        /// <summary>
        /// Objeto con el cual poder loguear información
        /// </summary>
        private readonly ILogger logger;
        /// <summary>
        /// Cliente Http a utilizar para hacer peticiones
        /// </summary>
        private readonly HttpClient httpClient;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="httpClient">Cliente Http a utilizar para hacer peticiones</param>
        /// <param name="logger">Objeto con el cual poder loguear información</param>
        public IsssteTramitesAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        #endregion

        #region Overiddes Methods

        /// <summary>
        /// Valida que si se llamo a la ruta de interpretación del midleware y de ser así, se detenga el procesamiento de los siguiente midlewares y se inicie el proceso de autenticación local
        /// </summary>
        /// <returns>True si se inicio el proceso de autenticación local, de lo contrario, false</returns>
        public override async Task<bool> InvokeAsync()
        {
            try
            {
                // This is always invoked on each request. For passive middleware, only do anything if this is
                // for our callback path when the user is redirected back from the authentication provider.

                if (Options.CallbackPath.HasValue && Request.Path == Options.CallbackPath)
                {
                    try
                    {
                        var ticket = await AuthenticateAsync();

                        if (ticket != null)
                        {
                            Context.Authentication.SignIn(ticket.Properties, ticket.Identity);

                            Response.Redirect(ticket.Properties.RedirectUri);

                            // Prevent further processing by the owin pipeline.
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        this.logger.WriteCore(TraceEventType.Error, 1, null, e, (s, ex) => JsonConvert.SerializeObject(ex));

                        Response.Redirect(Options.ErrorUrl);
                        return true;
                    }
                }

                // Let the rest of the pipeline run.
                return false;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        /// <summary>
        /// Procesa la respuesta del sistema de seguridad del ISSSTE y obtiene la información de usuario logueado
        /// </summary>
        /// <returns>Ticket de autenticación ara el usuario</returns>
        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            try
            {
                var userId = Request.Query["userId"];
                var accessToken = Request.Query["token"];
                var refreshToken = Request.Query["refreshToken"];
                var state = Request.Query["state"];

                var helper = new SecurityAuthenticationServiceAgent();

                var userInfo = await helper.GetUserInfoAsync(accessToken, Options.ProcedureId, userId);

                var context = new IsssteTramitesContext(this.Context, Options, state, userInfo, accessToken, refreshToken);

                return new AuthenticationTicket(context.AuthenticatedIdentity, context.AuthenticationProperties);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        /// <summary>
        /// Valida las respuestas emitidas por los controladores y busca una un <see cref="Web.IsssteChallengeResult"/> para iniciar el proceso de autenticación con el sistema de seguridad del ISSSTE
        /// </summary>
        protected override Task ApplyResponseChallengeAsync()
        {
            try
            {
                if (Response.StatusCode == 401)
                {
                    var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

                    // Only react to 401 if there is an authentication challenge for the authentication 
                    // type of this handler.
                    if (challenge != null)
                    {
                        var state = challenge.Properties;

                        if (string.IsNullOrEmpty(state.RedirectUri))
                        {
                            state.RedirectUri = Request.Uri.ToString();
                        }

                        state.Dictionary.Add("ProcedureId", Options.ProcedureId);
                        state.Dictionary.Add("ClientId", Options.ClientId);

                        var stateString = Options.StateDataFormat.Protect(state);

                        string returnUrl = string.Concat(new object[]
                            {
                              this.Request.Scheme,
                              Uri.SchemeDelimiter,
                              this.Request.Host,
                              this.Request.PathBase,
                              this.Options.CallbackPath
                            });

                        Response.Redirect(WebUtilities.AddQueryString(
                                    IsssteTramitesConstants.LoginUrl + "?isExternal=true",
                                    new Dictionary<string, string>
                                                {
                                                    { "returnUrl", returnUrl },
                                                    { "clientId", Options.ClientId },
                                                    { "state", stateString }
                                                }
                                    ));
                    }
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        #endregion
    }
}