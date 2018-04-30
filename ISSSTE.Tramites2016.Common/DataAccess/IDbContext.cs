using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.Common.Util;

namespace ISSSTE.Tramites2016.Common.DataAccess
{
    /// <summary>
    ///     Representa un <see cref="DbContext" />
    /// </summary>
    public interface IDbContext : IDisposable
    {
        #region DbContext Methods

        /// <summary>
        ///     Creates a Database instance for this context that allows for creation/deletion/existence
        ///     checks for the underlying database
        /// </summary>
        Database Database { get; }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        ///     Gets a System.Data.Entity.Infrastructure.DbEntityEntry object for the given entity
        ///     providing access to information about the entity and the ability to perform actions
        ///     on the entity.
        /// </summary>
        /// <param name="Entity">The entity</param>
        /// <returns>An entry for the entity</returns>
        DbEntityEntry Entry(object Entity);

        /// <summary />
        /// Returns a System.Data.Entity.DbSet`1 instance for access to entities of the given
        /// type in the context and the underlying store.
        /// <typeparam name="TEntity">The type entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        ///     Sets the entity as updated in a way that only only its not null properties wiill be sent to the database when
        ///     <see cref="SaveChangesAsync" /> is called
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="Entity">The entity</param>
        void SetEntityAsPartialyUpdated<T>(T Entity) where T : class;

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database, and applying "database wins"
        ///     approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException" /> occures
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="ConcurrencyResolution">The logic to use resolving the concurrency exception</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        Task<int> SaveChangesHandlingOptimisticConcurrencyAsync<T>(
            Extensions.ResolveConcurrency<T> ConcurrencyResolution) where T : class;

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database, and applying "database wins"
        ///     approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException" /> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        Task<int> SaveChangesHandlingOptimisticConcurrencyDatabaseWinsAsync<T>() where T : class;

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database, and applying "client wins"
        ///     approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException" /> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        Task<int> SaveChangesHandlingOptimisticConcurrencyClientWinsAsync<T>() where T : class;

        #endregion
    }
}