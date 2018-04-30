using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Security.ServiceAgents
{
    /// <summary>
    /// Contiene métodos que acceden a la información de los roles
    /// </summary>
    public interface ISecurityRolesServiceAgent
    {
        /// <summary>
        /// Obtiene el Id de un rol por su nombre
        /// </summary>
        /// <param name="owinContext">Contexto de Owin a utilizar</param>
        /// <param name="roleName">Nombre del rol</param>
        /// <returns>Id del rol (si existe), de lo contrario, null</returns>
        Task<Guid?> GetRoleIdByNameAsync(IOwinContext owinContext, string roleName);
    }
}
