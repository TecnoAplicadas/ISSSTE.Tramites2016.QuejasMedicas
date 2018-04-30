using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace ISSSTE.Tramites2016.Common.Security.Identity
{
    /// <summary>
    /// Tipo de <see cref="UserManager{TUser, TKey}"/> utilizado por las aplicaciones cliente del ISSSTE
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class IsssteUserManager<TUser> : UserManager<TUser>
        where TUser : IsssteIdentityUser, new()
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="store">Almacen de usuarios</param>
        public IsssteUserManager(IUserStore<TUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// Crea un nuevo <see cref="IsssteUserManager{TUser}"/>
        /// </summary>
        /// <param name="options">Opciones para la creación</param>
        /// <param name="context">Contexto Owin a utilizar</param>
        /// <returns>Nuevo user manager</returns>
        public static IsssteUserManager<TUser> Create(IdentityFactoryOptions<IsssteUserManager<TUser>> options, IOwinContext context)
        {
            var manager = new IsssteUserManager<TUser>(new UserStore<TUser>(context.Get<IsssteIdentityDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<TUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<TUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        /// <summary>
        /// Genera los claims de identidad de un usuario
        /// </summary>
        /// <param name="user">Usuario del cual crear claims</param>
        /// <param name="authenticationType">Método de autenticación</param>
        /// <returns>Claims de identidad del usuario</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(TUser user, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await this.CreateIdentityAsync(user, authenticationType);

            // Add custom user claims here

            return userIdentity;
        }
    }
}
