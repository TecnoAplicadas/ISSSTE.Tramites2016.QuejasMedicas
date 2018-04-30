using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Catalogs
{
    /// <summary>
    ///     Defines methods for quering a generic entity in a database
    /// </summary>
    public interface ICatalogRepository
    {
        /// <summary>
        ///     Returns a single object with a primary key of the provided id
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="keyValues">The primary key(s) of the object to fetch</param>
        /// <returns>A single object with the provided primary key or null</returns>
        Task<TObject> GetAsync<TObject>(params object[] keyValues) where TObject : class;

        /// <summary>
        ///     Gets a collection of all objects in the database
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>An ICollection of every object in the database</returns>
        Task<ICollection<TObject>> GetAllAsync<TObject>() where TObject : class;

        /// <summary>
        ///     Returns a single object which matches the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A Linq expression filter to find a single result</param>
        /// <returns>
        ///     A single object which matches the expression filter.
        ///     If more than one object is found or if zero are found, null is returned
        /// </returns>
        Task<TObject> FindAsync<TObject>(Expression<Func<TObject, bool>> match) where TObject : class;

        /// <summary>
        ///     Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An ICollection of object which match the expression filter</returns>
        Task<ICollection<TObject>> FindAllAsync<TObject>(Expression<Func<TObject, bool>> match) where TObject : class;

        /// <summary>
        ///     Inserts a single object to the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="t">The object to insert</param>
        /// <returns>The resulting object including its primary key after the insert</returns>
        Task<TObject> AddAsync<TObject>(TObject t) where TObject : class;

        /// <summary>
        ///     Inserts a collection of objects into the database and commits the changes
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="tList">An IEnumerable list of objects to insert</param>
        /// <returns>The IEnumerable resulting list of inserted objects including the primary keys</returns>
        Task<IEnumerable<TObject>> AddAllAsync<TObject>(IEnumerable<TObject> tList) where TObject : class;

        /// <summary>
        ///     Updates a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="updated">The updated object to apply to the database</param>
        /// <param name="key">The primary key of the object to update</param>
        /// <returns>The resulting updated object</returns>
        Task<TObject> UpdateAsync<TObject>(TObject updated, int key) where TObject : class;

        /// <summary>
        ///     Stores a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The resulting updated object</returns>
        Task<TObject> AddOrUpdateAsync<TObject>(TObject updated) where TObject : class;

        /// <summary>
        ///     Deletes a single object from the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="t">The object to delete</param>
        Task<int> DeleteAsync<TObject>(TObject t) where TObject : class;

        /// <summary>
        ///     Gets the count of the number of objects in the databse
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The count of the number of objects</returns>
        Task<int> CountAsync<TObject>() where TObject : class;
    }
}