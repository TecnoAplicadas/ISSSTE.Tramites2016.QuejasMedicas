using System;
using System.Reflection;

namespace ISSSTE.Tramites2016.Common.Catalogs
{
    /// <summary>
    ///     Denota una properiedad o clase que debe de mostrarse en las vistas de catalogos
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class CatalogDisplayAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        ///     El contructor de la clase
        /// </summary>
        public CatalogDisplayAttribute()
        {
            Order = int.MaxValue;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Obtiene el nombre a desplegar utilizando <see cref="ResourceType" /> y <see cref="DisplayNameResourceId" />
        /// </summary>
        /// <returns>Nombre a desplegar</returns>
        public string GetDisplayNameFromResource()
        {
            return GetResourceValue(ResourceType, DisplayNameResourceId);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        ///     Obtiene el valor de una propiedad de un recurso a partir de su tipo y el id específico
        /// </summary>
        /// <param name="resourceType">Tipo del recurso</param>
        /// <param name="resourceId">Id del recurso a extraer</param>
        /// <returns>Valor del recurso</returns>
        public string GetResourceValue(Type resourceType, string resourceId)
        {
            var displayName = "";

            if (resourceType != null && !string.IsNullOrEmpty(resourceId))
            {
                var resourceProperty = resourceType.GetProperty(resourceId, BindingFlags.Static | BindingFlags.Public);

                if (resourceProperty != null)
                    displayName = (string) resourceProperty.GetValue(resourceProperty.DeclaringType, null);
            }

            return displayName;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Obtiene o asigna el tipo del recurso a utilizar para obtener los nombres a desplegar
        /// </summary>
        public Type ResourceType { get; set; }

        /// <summary>
        ///     Obtiene o asigna nombre del recurso que contiene el nombre a desplegar
        /// </summary>
        public string DisplayNameResourceId { get; set; }

        /// <summary>
        ///     Obtiene o asigna el valor que indica si la propiedad se debe de utilizar como etiqueta cuando se es un campo
        ///     dependiente
        /// </summary>
        public bool IsLabel { get; set; }

        /// <summary>
        ///     Obtiene o asigna el valor que indica si el campo se debera de mostrar en la vista de listado
        /// </summary>
        public bool ShowInListView { get; set; }

        /// <summary>
        ///     Obtiene o asigna el orden en el que debe de mostrarse este elemento
        /// </summary>
        public int Order { get; set; }

        #endregion
    }
}