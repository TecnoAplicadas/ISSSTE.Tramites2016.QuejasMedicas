#region

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Common.Util
{
    /// <summary>
    ///     Metodos de extension para diversas clases
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Defines a method which resolves a concurrency exception in EF
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        public delegate T ResolveConcurrency<T>(T CurrentValues, T DatabaseValues);

        /// <summary>
        ///     Obtiene la descripción de un valor de un enumerador
        /// </summary>
        /// <param name="Value">Valor del enumerador</param>
        /// <returns>Descipción del valor del enumerador</returns>
        public static string GetEnumDescription(this Enum Value)
        {
            var fi = Value.GetType().GetField(Value.ToString());
            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            return Value.ToString();
        }

        /// <summary>
        ///     Obtiene el <see cref="Guid" /> del valor del enumerador
        /// </summary>
        /// <param name="Value">Valor del enumerador</param>
        /// <returns><see cref="Guid" /> del valor del enumerador</returns>
        public static Guid GetGuidAttribute(this Enum Value)
        {
            var fi = Value.GetType().GetField(Value.ToString());
            if (fi != null)
            {
                var attribute = (EnumGuidAttribute)
                    fi.GetCustomAttributes(typeof(EnumGuidAttribute), false)
                        .FirstOrDefault();
                if (attribute != null) return attribute.Guid;
            }

            return Guid.NewGuid();
        }

        /// <summary>
        ///     Sets the entity as updated in a way that only only its not null properties wiill be sent to the database when
        ///     <see>
        ///         <cref>SaveChangesAsync</cref>
        ///     </see>
        ///     is called
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="DbContext">The DbContext</param>
        /// <param name="Entity">The entity</param>
        public static void SetEntityAsPartialyUpdated<T>(this DbContext DbContext, T Entity) where T : class
        {
            //Se agrega al contexto y se marca como modificada
            var dbEntry = DbContext.Entry(Entity);
            dbEntry.State = EntityState.Modified;

            foreach (var name in dbEntry.CurrentValues.PropertyNames)
                //Si el nombre de la pripiedad coincied con la lista de propiedades, se marca como modificada
                if (dbEntry.Property(name).CurrentValue == null)
                {
                    dbEntry.Property(name).IsModified = false;

                    var property = typeof(T).GetProperty(name);

                    if (property != null &&
                        property.CustomAttributes.All(A => A.AttributeType != typeof(RequiredAttribute))) continue;
                    object value = null;

                    if (property != null && property.PropertyType == typeof(string))
                        value = "";

                    if (property != null) property.SetValue(Entity, value);
                }
        }


        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database, and applying custom logic if an
        ///     <see cref="System.Data.Entity.Core.OptimisticConcurrencyException" /> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="DbContext">The DbContext</param>
        /// <param name="ConcurrencyResolution">The logic to use resolving the concurrency exception</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        public static async Task<int> SaveChangesHandlingOptimisticConcurrencyAsync<T>(this DbContext DbContext,
            ResolveConcurrency<T> ConcurrencyResolution)
            where T : class
        {
            var result = 0;
            bool saveFailed;

            do
            {
                saveFailed = false;

                try
                {
                    result = await DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Get the current entity values and the values in the database 
                    // as instances of the entity type 
                    var entry = ex.Entries.Single();
                    var databaseValues = entry.GetDatabaseValues();
                    var databaseValuesAsObject = (T) databaseValues?.ToObject();

                    // Have the user choose what the resolved values should be 
                    var resolvedValuesAsBlog = ConcurrencyResolution((T) entry.Entity, databaseValuesAsObject);

                    // Update the original values with the database values and 
                    // the current values with whatever the user choose. 
                    if (databaseValues != null)
                        entry.OriginalValues.SetValues(databaseValues);
                    entry.CurrentValues.SetValues(resolvedValuesAsBlog);
                }
            } while (saveFailed);

            return result;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database, and applying "database wins"
        ///     approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException" /> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="DbContext">The DbContext</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        public static async Task<int> SaveChangesHandlingOptimisticConcurrencyDatabaseWinsAsync<T>(
            this DbContext DbContext)
            where T : class
        {
            return await SaveChangesHandlingOptimisticConcurrencyAsync<T>(DbContext,
                (CurrentValues, DatabaseValues) => DatabaseValues);
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database, and applying "client wins"
        ///     approach if an <see cref="System.Data.Entity.Core.OptimisticConcurrencyException" /> occurs
        /// </summary>
        /// <typeparam name="T">The type of the object involveed in the concurrency exception</typeparam>
        /// <param name="DbContext">The DbContext</param>
        /// <returns>1 if was successfull, 0 if was not successfull</returns>
        public static async Task<int> SaveChangesHandlingOptimisticConcurrencyClientWinsAsync<T>(
            this DbContext DbContext)
            where T : class
        {
            return await SaveChangesHandlingOptimisticConcurrencyAsync<T>(DbContext,
                (CurrentValues, DatabaseValues) => CurrentValues);
        }
    }
}