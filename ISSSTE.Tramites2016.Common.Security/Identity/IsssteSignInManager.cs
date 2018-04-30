using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace ISSSTE.Tramites2016.Common.Security.Identity
{
    /// <summary>
    /// Tipo de <see cref="SignInManager{TUser, TKey}"/> utilizado por las aplicaciones cliente del ISSSTE
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class IsssteSignInManager<TUser> : SignInManager<TUser, string>
        where TUser: IsssteIdentityUser, new()
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="userManager"><see cref="IsssteUserManager{TUser}"/> a utilizar</param>
        /// <param name="authenticationManager"><see cref="IAuthenticationManager"/> a utilizar</param>
        public IsssteSignInManager(IsssteUserManager<TUser> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        /// <summary>
        /// Genera los claims de identidad de un usuario
        /// </summary>
        /// <param name="user">Usuario del cual crear claims</param>
        /// <param name="authenticationType">Método de autenticación</param>
        /// <returns>Claims de identidad del usuario</returns>
        public Task<ClaimsIdentity> CreateUserIdentityAsync(TUser user, string authenticationType)
        {
            return ((IsssteUserManager<TUser>)UserManager).GenerateUserIdentityAsync(user, authenticationType);
        }

        /// <summary>
        /// Crea un nuevo <see cref="IsssteSignInManager{TUser}"/>
        /// </summary>
        /// <param name="options">Opciones para la creación</param>
        /// <param name="context">Contexto Owin a utilizar</param>
        /// <returns>Nuevo <see cref="IsssteSignInManager{TUser}"/> creado</returns>
        public static IsssteSignInManager<TUser> Create(IdentityFactoryOptions<IsssteSignInManager<TUser>> options, IOwinContext context)
        {
            return new IsssteSignInManager<TUser>(context.GetUserManager<IsssteUserManager<TUser>>(), context.Authentication);
        }
    }
}