using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace ISSSTE.Tramites2016.Common.Security.Identity
{
    /// <summary>
    /// Tipo de <see cref="RoleManager{TRole, TKey}"/> utilizado por las aplicaciones cliente del ISSSTE
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    public class IsssteRoleManager<TRole> : RoleManager<TRole>
        where TRole : IdentityRole, new()
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="roleStore">Almacen de los roles</param>
        public IsssteRoleManager(IRoleStore<TRole, string> roleStore)
            : base(roleStore)
        {
        }

        /// <summary>
        /// Crea un nuevo <see cref="IsssteRoleManager{TRole}"/>
        /// </summary>
        /// <param name="options">Opciones para la creación</param>
        /// <param name="context">Contexto Owin a utilizar</param>
        /// <returns>Nuevo <see cref="IsssteRoleManager{TRole}"/> creado</returns>
        public static IsssteRoleManager<TRole> Create(IdentityFactoryOptions<IsssteRoleManager<TRole>> options, IOwinContext context)
        {
            return new IsssteRoleManager<TRole>(new RoleStore<TRole>(context.Get<IsssteIdentityDbContext>()));
        }
    }
}