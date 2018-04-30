// Namespaces para la imagen de Captcha
using ISSSTE.Tramites2016.Common.Security.Helpers;
using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Owin;
using ISSSTE.Tramites2016.Common.Security.Web;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.LoginDTO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System;
using System.Linq;
using System.Security.Claims;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController:BaseController
    {

        #region Captcha

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        public ActionResult Captcha()
        {
            ViewBag.SiteKey = new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumCaptcha.siteKey);
            return PartialView();
        }

        #endregion Captcha

        #region LoginIssste

        /// <summary>
        ///     Despliega la página de login
        /// </summary>
        /// <param name="returnUrl">Url a la que redirigir una vez que se complete el logueo</param>
        /// <returns>Vista</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return base.HandleOperationExecution(() =>
            {
                ViewBag.ReturnUrl = returnUrl;

                return View();
            });
        }

        /// <summary>
        /// Procesa la respuesta del proveedor de identidad externo almacenando la información del usuario. En caso de se llamado por el sitio propio, inicia el proceso de logueo con el proveedor de identidad externo
        /// </summary>
        /// <param name="returnUrl">Url a la que redirigir una vez que se complete el logueo</param>
        /// <param name="error">Error generado en el proceso de logueo</param>
        /// <returns>Redirección</returns>
        [HttpGet]
        [AllowAnonymous]
        [NoAsyncTimeout]
        public async Task<ActionResult> ExternalLogin(string returnUrl, string error)
        {
            return await base.HandleOperationExecutionAsync<ActionResult>(async () =>
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;

                if (error != null)
                {
                    return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
                }

                var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();

                if (loginInfo == null && !User.Identity.IsAuthenticated)
                {
                    return new IsssteChallengeResult(IsssteTramitesConstants.DefaultAuthenticationType, Url.Action("ExternalLogin", "Account", new { ReturnUrl = returnUrl }));
                }

                if (loginInfo == null)
                    return Redirect(Url.Action("LoginError", "Account"));

                try
                {
                    var userManager = HttpContext.GetOwinContext().GetUserManager<IsssteUserManager<IsssteIdentityUser>>();
                    var roleManager = HttpContext.GetOwinContext().GetUserManager<IsssteRoleManager<IdentityRole>>();
                    var signInManager = HttpContext.GetOwinContext().Get<IsssteSignInManager<IsssteIdentityUser>>();

                    var loginResult = await IsssteAuthorizationHelper.LoginAsync(loginInfo, userManager, roleManager, signInManager, authenticationManager, Startup.ClientId);

                    await CancelacionAutomaticaDelDiaAsync();

                    await ReinicioDelContadorAsync();

                    //Token y Cookie
                    if (loginResult.Succeeded)
                    {

                        return Redirect(Url.Action("LoginComplete", "Account", new { ReturnUrl = returnUrl }));
                    }
                    else
                    {
                        var ex = new ArgumentException(loginResult.Errors.Single());

                        //base.LogException(ex);

                        return Redirect(Url.Action("LoginError", "Account"));
                    }
                }
                catch (Exception ex)
                {
                    //base.LogException(ex);

                    return Redirect(Url.Action("LoginError", "Account"));
                }
            });
        }

        /// <summary>
        ///     Despliega la´página que completa el logueo del lado del cliente
        /// </summary>
        /// <param name="returnUrl">Url a la que redirigir una vez que se complete el logueo</param>
        /// <returns>Vista</returns>
        [HttpGet]
        public ViewResult LoginComplete(string returnUrl)
        {
            return base.HandleOperationExecution(() =>
            {
                var owinContext = HttpContext.GetOwinContext();
                var userNameClaim =
                    owinContext.Authentication.User.Claims.FirstOrDefault(Claim => Claim.Type == ClaimTypes.Name);


                var model = new LoginModelDTO
                {
                    ClientId = Startup.ClientId,
                    UserName = userNameClaim == null ? "" : userNameClaim.Value,
                    ReturnUrl = returnUrl
                };

                return View(model);
            });
        }

        /// <summary>
        ///     Despliega la pagina de error en el logueo
        /// </summary>
        /// <returns>Vista</returns>
        [AllowAnonymous]
        [HttpGet]
        public ViewResult LoginError()
        {
            return base.HandleOperationExecution(() =>
            {
                return View();
            });
        }

        /// <summary>
        ///     Cierra sesión tanto en la cookie como en OAuth 2.0 y despliega la página de cierre de sesión
        /// </summary>
        /// <param name="returnUrl">Página a la que redirigir una vez cumplido el cierre de sesión</param>
        /// <param name="soft">Indica si tambien se debe de cerrar la sesión del proveedor de indetidad o no</param>
        /// <returns>Vista</returns>
        [AllowAnonymous]
        [HttpGet]
        public ViewResult Logout(string returnUrl, bool soft = false)
        {
            return base.HandleOperationExecution(() =>
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;

                authenticationManager.SignOut();

                ViewBag.Soft = soft;
                ViewBag.ReturnUrl = returnUrl;


                return View();
            });
        }

        #endregion LoginIssste

        #region OperacionesCitas

        /// <summary>
        /// Cancela las citas que son de dias pasados de manera automatica
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        private async Task CancelacionAutomaticaDelDiaAsync()
        {
            try
            {
                var citaService = new CitaService();
                await citaService.CancelacionAutomaticaDelDia();
            }
            catch (Exception ex)
            {
                EscribirLog(new ExceptionContext(ControllerContext, ex));
            }
        }

        /// <summary>
        /// Reinicia el contador, utilizado para la foliacion de las citas de manera automatica
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        private async Task ReinicioDelContadorAsync()
        {
            try
            {
                var citaService = new CitaService();
                await citaService.ReinicioContador();
            }
            catch (Exception ex)
            {
                EscribirLog(new ExceptionContext(ControllerContext, ex));
            }
        }

        #endregion OperacionesCitas


    }
}