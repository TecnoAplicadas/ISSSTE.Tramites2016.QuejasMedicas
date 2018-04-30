using Owin;

namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    using System;
    using System.Net.Http;
    using Microsoft.Owin;
    using Microsoft.Owin.Logging;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataHandler;
    using Microsoft.Owin.Security.DataProtection;
    using Microsoft.Owin.Security.Infrastructure;

    /// <summary>
    /// Middleware de autenticación de Owin que permite utilizar al sistema de seguridad del ISSSTE
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "OWN necesita tener el scope superior.")]
    public class IsssteTramitesAuthenticationMiddleware : AuthenticationMiddleware<IsssteTramitesAuthenticationOptions>
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
        /// <param name="next">Siguiente midelware en en pipeline</param>
        /// <param name="app">Apicación Owin</param>
        /// <param name="options">Opciones de autenticación a utilizar</param>
        public IsssteTramitesAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            IsssteTramitesAuthenticationOptions options)
            : base(next, options)
        {
            if (string.IsNullOrWhiteSpace(this.Options.ProcedureId))
            {
                throw new ArgumentException("ProcedureId");
            }
            else if (string.IsNullOrWhiteSpace(this.Options.ClientId))
            {
                throw new ArgumentException("ClientId");
            }
            else if (string.IsNullOrWhiteSpace(this.Options.Secret))
            {
                throw new ArgumentException("Secret");
            }
            else
            {
                if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
                {
                    options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
                }

                if (options.StateDataFormat == null)
                {
                    var dataProtector =
                        app.CreateDataProtector(
                                            typeof(IsssteTramitesAuthenticationMiddleware).FullName,
                                            options.AuthenticationType);

                    options.StateDataFormat = new PropertiesDataFormat(dataProtector);
                }

                this.logger = app.CreateLogger<IsssteTramitesAuthenticationMiddleware>();

                this.httpClient = new HttpClient(IsssteTramitesAuthenticationMiddleware.ResolveHttpMessageHandler(this.Options));
                this.httpClient.Timeout = this.Options.BackchannelTimeout;
                this.httpClient.MaxResponseContentBufferSize = 10485760L;
            }
        }

        #endregion

        #region Overrided Methods

        /// <summary>
        /// Crea un manejador de autenticaciónpor cada petición
        /// </summary>
        /// <returns>Manejador de autenticación creado</returns>
        protected override AuthenticationHandler<IsssteTramitesAuthenticationOptions> CreateHandler()
        {
            return new IsssteTramitesAuthenticationHandler(this.httpClient, this.logger);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Construye un nuevo <see cref="HttpMessageHandler"/> en base a las opciones de autenticación
        /// </summary>
        /// <param name="options">Opciones de autenticación</param>
        /// <returns>Nuevo <see cref="HttpMessageHandler"/> creado</returns>
        private static HttpMessageHandler ResolveHttpMessageHandler(IsssteTramitesAuthenticationOptions options)
        {
            HttpMessageHandler httpMessageHandler = options.BackchannelHttpHandler ?? (HttpMessageHandler)new WebRequestHandler();

            // if (options.BackchannelCertificateValidator != null)
            // {
            //     WebRequestHandler webRequestHandler = httpMessageHandler as WebRequestHandler;
            //     if (webRequestHandler == null)
            //         throw new InvalidOperationException(Resources.Exception_ValidatorHandlerMismatch);
            //     webRequestHandler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(options.BackchannelCertificateValidator.Validate);
            // }
            return httpMessageHandler;
        }

        #endregion
    }
}