using ISSSTE.Tramites2016.Common.Security.Model;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Security.ServiceAgents
{
    /// <summary>
    /// Contiene métodos que acceden a la información de los usuarios
    /// </summary>
    public interface ISecurityUsersServiceAgent
    {
        /// <summary>
        /// Obtiene la información mínima de los uaurios de un rol dentro de un trámite
        /// </summary>
        /// <param name="owinContext">Contexto Owin a utilizar</param>
        /// <param name="procedureId">Id del trámite</param>
        /// <param name="roleId">Id del rol</param>
        /// <returns>Lista de usuarios</returns>
        Task<List<SimpleUser>> GetUsersByProcedureAndRoleAsync(IOwinContext owinContext, Guid procedureId, Guid roleId);
    }
}
