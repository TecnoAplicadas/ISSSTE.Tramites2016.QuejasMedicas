using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System;

namespace ISSSTE.Tramites2016.Common.Security.Identity
{
    /// <summary>
    /// Tipo de <see cref="IdentityDbContext"/>utilizado por las aplicaciones cliente del ISSSTE
    /// </summary>
    public class IsssteIdentityDbContext : IdentityDbContext<IsssteIdentityUser>
    {
        #region Constructor

        /// <summary>
        /// Constructor de la clase que usa la cadena de conexión por default
        /// </summary>
        public IsssteIdentityDbContext()
            : base("IdentityConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<IsssteIdentityDbContext>(null);
        }

        /// <summary>
        /// Método preparativo llamado al moemnto de la creación de un <see cref="DbContext"/>
        /// </summary>
        /// <param name="modelBuilder">Contenedor de la configuración de creación</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var defaultSchema = ConfigurationManager.AppSettings["DatabaseSchema"];

            if (!String.IsNullOrEmpty(defaultSchema))
                modelBuilder.HasDefaultSchema(defaultSchema);

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Crea un nuevo <see cref="IsssteIdentityDbContext"/>
        /// </summary>
        /// <returns>El nuevo contexto</returns>
        public static IsssteIdentityDbContext Create()
        {
            return new IsssteIdentityDbContext();
        }

        #endregion
    }
}