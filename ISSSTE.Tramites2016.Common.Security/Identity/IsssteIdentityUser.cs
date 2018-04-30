using Microsoft.AspNet.Identity.EntityFramework;

namespace ISSSTE.Tramites2016.Common.Security.Identity
{
    /// <summary>
    /// Extiende la información de <see cref="IdentityUser"/> agregandole un nombre
    /// </summary>
    public class IsssteIdentityUser : IdentityUser
    {
        /// <summary>
        /// Obtiene o asigna el nombre del usuario
        /// </summary>
        public string Name { get; set; }
    }
}