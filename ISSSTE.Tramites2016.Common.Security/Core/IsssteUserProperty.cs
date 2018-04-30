namespace ISSSTE.Tramites2016.Common.Security.Core
{
    /// <summary>
    ///     Representa la información de un propiedad de un usuario
    /// </summary>
    public class IsssteUserProperty
    {
        /// <summary>
        ///     Obtiene o asigna el Id de la propiedad
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Obtiene o asigna el tipo de la propiedad
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Obtiene o asigna el nombre de la propiedad
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Obtiene o asigna el valor de la propiedad
        /// </summary>
        public string Value { get; set; }
    }
}