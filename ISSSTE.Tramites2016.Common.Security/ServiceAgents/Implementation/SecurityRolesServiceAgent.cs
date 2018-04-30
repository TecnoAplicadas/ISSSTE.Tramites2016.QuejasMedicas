using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Owin;
using Newtonsoft.Json;
using ISSSTE.Tramites2016.Common.Security.Model;
using System.Web;

namespace ISSSTE.Tramites2016.Common.Security.ServiceAgents.Implementation
{
    /// <summary>
    /// Implementación de <see cref="ISecurityRolesServiceAgent"/>
    /// </summary>
    public class SecurityRolesServiceAgent : BaseSecurityServiceAgent, ISecurityRolesServiceAgent
    {
        #region Properties

        /// <summary>
        /// Plantilla de la url relativa donde obtener el id de un role por su nombre
        /// </summary>
        private const string RoleIdPathTemplate = "api/Roles/Id?name={0}";

        #endregion

        #region IRolesServiceAgent Implementation

        public async Task<Guid?> GetRoleIdByNameAsync(IOwinContext owinContext, string roleName)
        {
            var accessToken = base.GetUserAccessToken<IsssteIdentityUser>(owinContext);

            var address = IsssteTramitesConstants.BaseUrl + String.Format(RoleIdPathTemplate, HttpUtility.UrlEncode(roleName));

            var httpClient = BuildHttpClient(address, accessToken);

            var response = await httpClient.GetAsync(address);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            var roleId = JsonConvert.DeserializeObject<Guid?>(responseContent);

            return roleId;
        }

        #endregion        
    }
}
