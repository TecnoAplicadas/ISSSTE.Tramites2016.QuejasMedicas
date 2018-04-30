using ISSSTE.Tramites2016.Common.Security.Claims;
using ISSSTE.Tramites2016.Common.Security.Core;
using ISSSTE.Tramites2016.Common.Web;
using Microsoft.Owin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ISSSTE.Tramites2016.Common.Security.Helpers
{
    /// <summary>
    /// Define métodos para la obtención de la identidad de un usuario ISSSTE logueado en un <see cref="IOwinContext"/>
    /// </summary>
    public static class OwinExtensions
    {
        /// <summary>
        /// Obtiene la información de un usuario autenticado bajo el contexto owin actual
        /// </summary>
        /// <param name="owinContext">El contexto owin actual</param>
        /// <returns>Información de usuario autenticado</returns>
        public static IsssteUser GetAuthenticatedUser(this IOwinContext owinContext)
        {
            if (owinContext.Authentication.User.Identity.IsAuthenticated == false)
            {
                return null;
            }

            var idClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            var userNameClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
            var givenNameClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName);
            var emailClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            var user = new Core.IsssteUser
            {
                Id = idClaim == null ? string.Empty : idClaim.Value,
                UserName = userNameClaim == null ? string.Empty : userNameClaim.Value,
                Name = givenNameClaim == null ? string.Empty : givenNameClaim.Value,
                Email = emailClaim == null ? string.Empty : emailClaim.Value
            };

            var propertiesClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == IsssteTramitesClaimTypes.Properties);

            List<IsssteUserProperty> properties = new List<IsssteUserProperty>();

            if (propertiesClaim != null)
                properties = JsonConvert.DeserializeObject<List<IsssteUserProperty>>(propertiesClaim.Value);

            user.Properties = properties;

            user.Roles = owinContext.GetAuthenticatedUserRoles();

            return user;
        }

        /// <summary>
        /// Obtiene la lista de roles del usuario autenticado bajo el contexto Owin actual
        /// </summary>
        /// <param name="owinContext">El contexto owin actual</param>
        /// <returns>Lista de roles del usuario autenticado</returns>
        public static List<IsssteRole> GetAuthenticatedUserRoles(this IOwinContext owinContext)
        {
            if (owinContext.Authentication.User.Identity.IsAuthenticated == false)
            {
                return null;
            }

            var rolesClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == IsssteTramitesClaimTypes.Roles);

            List<IsssteRole> roles = null;

            if (rolesClaim != null)
            {
                roles = JsonConvert.DeserializeObject<List<IsssteRole>>(rolesClaim.Value);
            }

            return roles;
        }

        /// <summary>
        /// Obtiene la lista de roles del usuario, que esta autenticado en el contexto de Owin, que coinciden con la lista de roles definidos en en el atributo 
        /// <see cref="Common.Web.Mvc.AuthorizeByConfigAttribute"/> o <see cref="Common.Web.Http.AuthorizeByConfigAttribute"/> aplicado al método que ejecuta la
        /// llamada a este método
        /// </summary>
        /// <remarks>Si el método que realiza la llamada no tiene aplicado ninguno de los atributos de segurdad mencionados, se regresaran todos los roles del usuario</remarks>
        /// <param name="owinContext">El contexto owin actual</param>
        /// <param name="callingMemberName">Nombre del método que esta ejecutando esta llamada</param>
        /// <returns>Lista de roles del usuario autenticado que coinciden con los roles autorizados</returns>
        public static List<IsssteRole> GetAuthenticatedUserAuthorizedRoles(this IOwinContext owinContext, [CallerMemberName]string callingMemberName = "")
        {
            List<IsssteRole> result = new List<IsssteRole>();

            var allUserRoles = owinContext.GetAuthenticatedUserRoles();

            StackTrace stackTrace = new StackTrace();

            var callingTypeMethod = stackTrace.GetFrames()
                .Where(f => f.GetMethod().Name == callingMemberName)
                .Select(f => f.GetMethod())
                .LastOrDefault();

            if (callingTypeMethod != null)
            {
                var authorizeAttribute = (IAuthorizeByConfig)callingTypeMethod.GetCustomAttributes(typeof(Common.Web.Mvc.AuthorizeByConfigAttribute), false).FirstOrDefault();

                if (authorizeAttribute == null)
                    authorizeAttribute = (IAuthorizeByConfig)callingTypeMethod.GetCustomAttributes(typeof(Common.Web.Http.AuthorizeByConfigAttribute), false).FirstOrDefault();

                if (authorizeAttribute != null)
                {
                    var authorizationRoles = authorizeAttribute.GetRoles();

                    result = allUserRoles
                        .Where(r => authorizationRoles.Contains(r.Name))
                        .ToList();
                }
            }
            else
                result = allUserRoles;

            return result;
        }


        //Se agrega método Integración --UPG
        public static List<string> GetAuthenticatedRol(this IOwinContext owinContext)
        {
            List<string> role = new List<string>();
            if (owinContext.Authentication.User.Identity.IsAuthenticated == false)
            {
                return null;
            }

            var rolesClaim = owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == IsssteTramitesClaimTypes.Roles);

            List<IsssteRole> roles = null;

            if (rolesClaim != null)
            {
                roles = JsonConvert.DeserializeObject<List<IsssteRole>>(rolesClaim.Value);
            }

            foreach (var rol in roles)
            {
                role.Add(rol.Name);
            }


            return role;

        }
    }
}
