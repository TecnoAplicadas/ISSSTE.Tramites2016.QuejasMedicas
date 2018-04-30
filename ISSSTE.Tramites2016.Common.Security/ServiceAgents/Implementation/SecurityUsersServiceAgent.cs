using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Model;
using ISSSTE.Tramites2016.Common.Security.Owin;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Security.ServiceAgents.Implementation
{
    /// <summary>
    /// Implementación de  <see cref="ISecurityUsersServiceAgent"/>
    /// </summary>
    public class SecurityUsersServiceAgent : BaseSecurityServiceAgent, ISecurityUsersServiceAgent
    {
        #region Properties

        /// <summary>
        /// Plantilla de la url relativa de donde obtener la lita de usuario de un rol dentro de un trámite
        /// </summary>
        private const string ProcedureRoleUsersPathTemplate = "api/Procedures/{0}/Roles/{1}/Users";

        #endregion

        #region IUsersServiceAgent

        public async Task<List<SimpleUser>> GetUsersByProcedureAndRoleAsync(IOwinContext owinContext, Guid procedureId, Guid roleId)
        {
            var accessToken = base.GetUserAccessToken<IsssteIdentityUser>(owinContext);

            var address = IsssteTramitesConstants.BaseUrl + String.Format(ProcedureRoleUsersPathTemplate, procedureId, roleId);

            var httpClient = BuildHttpClient(address, accessToken);

            var response = await httpClient.GetAsync(address);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<List<SimpleUser>>(responseContent);

            return users;
        }

        #endregion
    }
}
