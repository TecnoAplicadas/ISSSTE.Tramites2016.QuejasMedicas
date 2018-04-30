using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Cookies;
using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Core;
using ISSSTE.Tramites2016.Common.Security.Claims;

namespace ISSSTE.Tramites2016.Common.Security.Helpers
{
    /// <summary>
    /// Tiene métodos auxiliares para procesos de autorización de usuarios
    /// </summary>
    public static class IsssteAuthorizationHelper
    {
        /// <summary>
        /// Realiza el proceso de inicio de sesión en ASP Identity
        /// </summary>
        /// <typeparam name="TUser">Tipo del usuario</typeparam>
        /// <typeparam name="TRole">Tipo del rol</typeparam>
        /// <param name="loginInfo">Información de logue enviada por el sistema de segurdad ISSSTE</param>
        /// <param name="userManager"><see cref="IsssteUserManager{TUser}"/> a utilizar</param>
        /// <param name="roleManager"><see cref="RoleManager{TRole, TKey}"/> a utilizar</param>
        /// <param name="signInManager"><see cref="SignInManager{TUser, TKey}"/> a utilizar</param>
        /// <param name="authenticationManager"><see cref="IAuthenticationManager"/> a utilizar</param>
        /// <param name="clientId">Id del cliente con el que se esta iniciando sesión</param>
        /// <returns></returns>
        public static async Task<IdentityResult> LoginAsync<TUser, TRole>(ExternalLoginInfo loginInfo, IsssteUserManager<TUser> userManager, RoleManager<TRole> roleManager, SignInManager<TUser, string> signInManager, IAuthenticationManager authenticationManager, string clientId)
            where TUser : Identity.IsssteIdentityUser, new()
            where TRole : IdentityRole, new()
        {
            TUser user = null;
            IdentityUserClaim rolesClaim = null;

            //Usuario
            user = await userManager.FindByNameAsync(loginInfo.DefaultUserName);

            if (user != null)
                await userManager.DeleteAsync(user);

            var nameClaim = loginInfo.ExternalIdentity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName);

            user = new TUser
            {
                UserName = loginInfo.DefaultUserName,
                Name = nameClaim == null ? loginInfo.DefaultUserName : nameClaim.Value,
                Email = loginInfo.Email
            };

            var result = await userManager.CreateAsync(user);

            //Claims en memoria
            if (result.Succeeded)
            {
                foreach (var claim in loginInfo.ExternalIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier || claim.Type == ClaimTypes.Name)
                        continue;

                    var userClaim = new IdentityUserClaim
                    {
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value
                    };

                    user.Claims.Add(userClaim);

                    if (claim.Type == IsssteTramitesClaimTypes.Roles)
                        rolesClaim = userClaim;
                }
            }

            //Roles
            if (result.Succeeded && rolesClaim != null)
            {
                var roles = JsonConvert.DeserializeObject<List<IsssteRole>>(rolesClaim.ClaimValue);

                foreach (var role in roles)
                {
                    var applicationRole = roleManager.FindByName(role.Name);

                    if (applicationRole == null)
                    {
                        applicationRole = new TRole();
                        applicationRole.Name = role.Name;

                        result = await roleManager.CreateAsync(applicationRole);

                        if (!result.Succeeded)
                            break;
                    }
                }

                if (result.Succeeded)
                {
                    result = await userManager.UpdateAsync(user);

                    //Relación de usuario a rol
                    if (result.Succeeded)
                        result = await userManager.AddToRolesAsync(user.Id, roles.Select(r => r.Name).ToArray());
                }
            }

            //Login
            if (result.Succeeded)
                result = await userManager.AddLoginAsync(user.Id, loginInfo.Login);

            //SigIn
            if (result.Succeeded)
            {
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await userManager.GenerateUserIdentityAsync(user, OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await userManager.GenerateUserIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = IsssteOAuthProvider<TUser>.CreateProperties(user.Id, user.UserName, user.Email, clientId);
                authenticationManager.SignIn(properties, oAuthIdentity, cookieIdentity);

                //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            }

            return result;
        }

    }
}