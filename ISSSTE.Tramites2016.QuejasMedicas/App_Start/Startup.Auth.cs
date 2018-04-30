using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Owin;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public partial class Startup
    {
        #region Static Properties

        public static string ClientId
        {
            get { return ConfigurationManager.AppSettings["ClientId"]; }
        }

        public static string ProcedureId
        {
            get { return ConfigurationManager.AppSettings["ProcedureId"]; }
        }

        public static string Secret
        {
            get { return ConfigurationManager.AppSettings["Secret"]; }
        }

        public static string CookieName
        {
            get { return ConfigurationManager.AppSettings["CookieName"]; }
        }

        #endregion

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(IsssteIdentityDbContext.Create);
            app.CreatePerOwinContext<IsssteUserManager<IsssteIdentityUser>>(IsssteUserManager<IsssteIdentityUser>.Create);
            app.CreatePerOwinContext<IsssteRoleManager<IdentityRole>>(IsssteRoleManager<IdentityRole>.Create);
            app.CreatePerOwinContext<IsssteSignInManager<IsssteIdentityUser>>(IsssteSignInManager<IsssteIdentityUser>.Create);

            //Se utiliza para "darle la vuelta" a un error de asignación de cookies de autenticación en OWIN
            app.UseKentorOwinCookieSaver();

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                CookieName = Startup.CookieName,
                LoginPath = new PathString("/account/login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(Double.Parse(ConfigurationManager.AppSettings["TokenTimeoutMinutes"]))
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new IsssteOAuthProvider<IsssteIdentityUser>(Startup.ClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Double.Parse(ConfigurationManager.AppSettings["TokenTimeoutMinutes"])),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            //Configures the application to use the ISSSTE Identity Provider
            app.UseIsssteTramitesAuthentication(
                procedureId: Startup.ProcedureId,
                clientId: ClientId,
                secret: Startup.Secret,
                errorUrl: "account/loginerror");
        }


    }
}