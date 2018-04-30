using System;

namespace ISSSTE.Tramites2016.Common.Catalogs
{
    /// <summary>
    ///     Represents the metadata of a property from a catalog type
    /// </summary>
    public class CatalogPropertyInfo
    {
        /// <summary>
        ///     Gets or sets the property system name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the property type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     Gets or sets the property display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the value indicating if the property is foreing key or not
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        ///     Gets or sets the value indicating if the property is required or not
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the order in which the properti should be placed
        /// </summary>
        public int Order { get; set; }
    }
}