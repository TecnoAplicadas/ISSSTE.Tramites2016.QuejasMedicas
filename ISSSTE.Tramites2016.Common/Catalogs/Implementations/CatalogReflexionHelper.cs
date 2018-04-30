using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Catalogs.Implementations
{
    /// <summary>
    ///     Defines methods that help with the generic processing of catalogs using reflexion
    /// </summary>
    public class CatalogReflexionHelper : ICatalogReflexionHelper
    {
        #region Fields

        private readonly string _modelNamespace;

        #endregion

        #region Constructor

        /// <summary>
        ///     Class constructor
        /// </summary>
        /// <param name="modelNamespace">Name of the namespace of the catalogs classes</param>
        public CatalogReflexionHelper(string modelNamespace)
        {
            _modelNamespace = modelNamespace;
        }

        #endregion

        #region Constants

        /// <summary>
        ///     The name of the method <see cref="ICatalogRepository.GetAllAsync{TObject}" />
        /// </summary>
        private const string GetAllAsyncMethodName = "GetAllAsync";

        /// <summary>
        ///     The name of the method <see cref="ICatalogRepository.GetAsync{TObject}(object[])" />
        /// </summary>
        private const string GetAsyncMethodName = "GetAsync";

        #endregion

        #region ICatalogReflexionHelper Implementation

        public Type GetType(string catalogName)
        {
            var typeName = _modelNamespace + "." + SingularizeWord(catalogName);
            var result = Type.GetType(typeName);

            if (result == null)
                foreach (var actualAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    result = actualAssembly.GetType(typeName);

                    if (result != null)
                        break;
                }

            return result;
        }

        public string GetCatalogDisplayName(string catalogName)
        {
            var name = catalogName;

            var catalogType = GetType(catalogName);

            var displayAttribute = catalogType.GetCustomAttribute<CatalogDisplayAttribute>();

            if (displayAttribute != null)
                name = displayAttribute.GetDisplayNameFromResource();

            return name;
        }

        public string GetCatalogKeyPropertyName(string catalogName)
        {
            var keyPropertyName = "";

            var catalogType = GetType(catalogName);

            foreach (var actualProperty in catalogType.GetProperties())
            {
                var keyAttribute = actualProperty.GetCustomAttribute<KeyAttribute>();

                if (keyAttribute != null)
                    keyPropertyName = actualProperty.Name;
            }

            return keyPropertyName;
        }

        public List<CatalogPropertyInfo> GetPropertiesToDisplayInListView(string catalogName)
        {
            return GetPropertiesToDisplay(catalogName, true);
        }

        public List<CatalogPropertyInfo> GetPropertiesToDisplayInDetailView(string catalogName)
        {
            return GetPropertiesToDisplay(catalogName, false);
        }

        public async Task<T> InvokeGetAllAsync<T>(ICatalogRepository repository, string catalogName)
        {
            return await InvokeAsyncGenericMethod<T>(repository, GetAllAsyncMethodName, catalogName, null);
        }

        public async Task<T> InvokeGetAsync<T>(ICatalogRepository repository, string catalogName, string key)
        {
            var convertedParameter = ConvertToMostSuitableType(key);

            return await InvokeAsyncGenericMethod<T>(repository, GetAsyncMethodName, catalogName,
                new object[] {new[] {convertedParameter}});
        }

        public async Task<Dictionary<string, List<dynamic>>> GetTypeDependentPropertiesAsync(
            ICatalogRepository domainService, string catalogName, List<string> dependentPropertyNames)
        {
            var result = new Dictionary<string, List<dynamic>>();

            var catalogType = GetType(catalogName);

            if (dependentPropertyNames != null)
                foreach (var actualDependentPropertyName in dependentPropertyNames)
                {
                    var formattedList = new List<dynamic>();

                    var actualDependentProperty = catalogType.GetProperty(actualDependentPropertyName);
                    var foreignKeyAttribute = actualDependentProperty.GetCustomAttribute<ForeignKeyAttribute>();

                    if (foreignKeyAttribute != null)
                    {
                        var navigationProperty = catalogType.GetProperty(foreignKeyAttribute.Name);

                        if (navigationProperty != null)
                        {
                            var dependentList =
                                await InvokeGetAllAsync<IEnumerable<object>>(domainService,
                                    navigationProperty.PropertyType.Name);

                            formattedList = dependentList.Select(e =>
                                {
                                    var idProperty = e
                                        .GetType()
                                        .GetProperties()
                                        .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);

                                    var namePropertys = e.GetType()
                                        .GetProperties()
                                        .Where(p => p.GetCustomAttribute<CatalogDisplayAttribute>() != null &&
                                                    p.GetCustomAttribute<CatalogDisplayAttribute>().IsLabel)
                                        .ToList();

                                    var nameValue = new StringBuilder();

                                    foreach (var actualNameProperty in namePropertys)
                                        nameValue.Append(actualNameProperty.GetValue(e) + " ");

                                    return new
                                    {
                                        Id = idProperty != null ? idProperty.GetValue(e) : null,
                                        Name = nameValue.ToString()
                                    };
                                })
                                .Cast<dynamic>()
                                .ToList();
                        }
                    }

                    result.Add(actualDependentPropertyName, formattedList);
                }

            return result;
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Gets al the properties with <see cref="CatalogDisplayAttribute" /> from a catalog
        /// </summary>
        /// <param name="catalogName">The catalog type</param>
        /// <param name="showListViewPropertiesOnly">
        ///     Indicates if show only properties marked with
        ///     <see cref="CatalogDisplayAttribute.ShowInListView" />
        /// </param>
        /// <returns>The properties to display</returns>
        public List<CatalogPropertyInfo> GetPropertiesToDisplay(string catalogName, bool showListViewPropertiesOnly)
        {
            var result = new List<CatalogPropertyInfo>();

            var catalogType = GetType(catalogName);

            foreach (var actualProperty in catalogType.GetProperties())
            {
                var displayAttribute = actualProperty.GetCustomAttribute<CatalogDisplayAttribute>();

                if (displayAttribute != null)
                {
                    var requiredAttribute = actualProperty.GetCustomAttribute<RequiredAttribute>();
                    var foreignKeyAttribute = actualProperty.GetCustomAttribute<ForeignKeyAttribute>();

                    var propertyInfo = new CatalogPropertyInfo
                    {
                        Name = actualProperty.Name,
                        Type = actualProperty.PropertyType,
                        DisplayName = displayAttribute.GetDisplayNameFromResource(),
                        IsForeignKey = foreignKeyAttribute != null,
                        IsRequired = requiredAttribute != null,
                        Order = displayAttribute.Order
                    };

                    if (showListViewPropertiesOnly && !displayAttribute.ShowInListView)
                        continue;

                    result.Add(propertyInfo);
                }
            }

            result = result.OrderBy(cp => cp.Order).ToList();

            return result;
        }

        /// <summary>
        ///     Trys to convert the supplied parametrs to varoius types trying to infer the true type of the parameter
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <returns>The converted parameter</returns>
        private object ConvertToMostSuitableType(string parameter)
        {
            object result;

            Guid guid;
            DateTime date;
            bool boolean;
            int integer;

            if (Guid.TryParse(parameter, out guid))
                result = guid;
            else if (DateTime.TryParse(parameter, out date))
                result = date;
            else if (bool.TryParse(parameter, out boolean))
                result = boolean;
            else if (int.TryParse(parameter, out integer))
                result = integer;
            else
                result = parameter;

            return result;
        }

        /// <summary>
        ///     Invokes dinamically a method in <see cref="ICatalogRepository" />
        /// </summary>
        /// <typeparam name="T">Type of the return object</typeparam>
        /// <param name="repository">The instance of <see cref="ICatalogRepository" /> to use in the invocation</param>
        /// <param name="methodName"></param>
        /// <param name="catalogName">The name of the catalog class to use</param>
        /// <param name="parameters">Parameters to use in the invocation of thye method</param>
        /// <returns>Result of the method called</returns>
        private async Task<T> InvokeAsyncGenericMethod<T>(ICatalogRepository repository, string methodName,
            string catalogName, params object[] parameters)
        {
            var genericMethod = CreateGenericMethod(catalogName, methodName);

            // build the task
            dynamic resultTask = (Task) genericMethod.Invoke(repository, parameters);

            // execute the task
            await resultTask;

            // execute the task
            return (T) resultTask.Result;
        }

        /// <summary>
        ///     Get the task to execute for the execution of the method
        /// </summary>
        /// <param name="catalogName">Catalog name to invoke</param>
        /// <param name="methodName">Method name to invoke</param>
        /// <returns></returns>
        private MethodInfo CreateGenericMethod(string catalogName, string methodName)
        {
            var catalogInstance = Activator.CreateInstance(GetType(catalogName));

            // get the method
            var method = typeof(ICatalogRepository).GetMethod(methodName);

            // make it generic
            var genericMethod = method.MakeGenericMethod(catalogInstance.GetType());

            return genericMethod;
        }

        /// <summary>
        ///     Singularize the given word
        /// </summary>
        /// <param name="word">Word to sigularize</param>
        /// <returns></returns>
        private string SingularizeWord(string word)
        {
            var pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

            return pluralizationService.IsPlural(word) ? pluralizationService.Singularize(word) : word;
        }

        #endregion
    }
}