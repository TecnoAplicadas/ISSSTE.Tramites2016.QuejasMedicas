using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Catalogs
{
    /// <summary>
    ///     Defines methods that help with the generic processing of catalogs using reflexion
    /// </summary>
    public interface ICatalogReflexionHelper
    {
        /// <summary>
        ///     Get a type from a name
        /// </summary>
        /// <param name="catalogName">Name of the catalog to serialize</param>
        /// <returns>The <see cref="Type" /> of the catalog if found, otherwise null</returns>
        Type GetType(string catalogName);

        /// <summary>
        ///     Gets the display name of a catalog
        /// </summary>
        /// <param name="catalogName">The catalog type</param>
        /// <returns>The display name</returns>
        string GetCatalogDisplayName(string catalogName);

        /// <summary>
        ///     Gets the key property of the catalog type
        /// </summary>
        /// <param name="catalogName">The catalog type</param>
        /// <returns>The key property</returns>
        string GetCatalogKeyPropertyName(string catalogName);

        /// <summary>
        ///     Get the property informations from those properties to show in a list view (every display property)
        /// </summary>
        /// <param name="catalogName">The catalog type</param>
        /// <returns>The properties to display</returns>
        List<CatalogPropertyInfo> GetPropertiesToDisplayInListView(string catalogName);

        /// <summary>
        ///     Get the property informations from those properties to show in a ditail view (every display property)
        /// </summary>
        /// <param name="catalogName">The catalog type</param>
        /// <returns>The properties to display</returns>
        List<CatalogPropertyInfo> GetPropertiesToDisplayInDetailView(string catalogName);

        /// <summary>
        ///     Invokes dynamically <see cref="ICatalogRepository.GetAllAsync{TObject}" /> with the information provided
        /// </summary>
        /// <typeparam name="T">Type of the return object</typeparam>
        /// <param name="repository">The instance of <see cref="ICatalogRepository" /> to use in the invocation</param>
        /// <param name="catalogName">The name of the catalog class to use</param>
        /// <returns>Result of <see cref="ICatalogRepository.GetAllAsync{TObject}" /> where TObject is typeof(catalogName)</returns>
        Task<T> InvokeGetAllAsync<T>(ICatalogRepository repository, string catalogName);

        /// <summary>
        ///     Invokes dynamically <see cref="ICatalogRepository.GetAsync{TObject}(object[])" /> with the information provided
        /// </summary>
        /// <typeparam name="T">Type of the return object</typeparam>
        /// <param name="repository">The instance of <see cref="ICatalogRepository" /> to use in the invocation</param>
        /// <param name="catalogName">The name of the catalog class to use</param>
        /// <param name="key">
        ///     Parameter to use in the invocation of <see cref="ICatalogRepository.GetAsync{TObject}(object[])" />
        /// </param>
        /// <returns>Result of <see cref="ICatalogRepository.GetAsync{TObject}(object[])" /> where TObject is typeof(catalogName)</returns>
        Task<T> InvokeGetAsync<T>(ICatalogRepository repository, string catalogName, string key);

        /// <summary>
        ///     Gets a dictionary of {Id, Name} with the posible elements of each dependent property in typeof(catalogName), using
        ///     the names in dependentPropertyNames
        /// </summary>
        /// <param name="domainService">The instance of <see cref="ICatalogRepository" /> to use in the invocation</param>
        /// <param name="catalogName">The name of the catalog class to use</param>
        /// <param name="dependentPropertyNames">Name of the properties to search throught in the class</param>
        /// <returns>List of dependent values per property</returns>
        Task<Dictionary<string, List<dynamic>>> GetTypeDependentPropertiesAsync(ICatalogRepository domainService,
            string catalogName, List<string> dependentPropertyNames);
    }
}