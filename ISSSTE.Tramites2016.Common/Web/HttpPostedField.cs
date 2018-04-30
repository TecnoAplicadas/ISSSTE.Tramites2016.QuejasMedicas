namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    ///     Representa un campo enviado como multipart
    /// </summary>
    public class HttpPostedField
    {
        #region Constructor

        /// <summary>
        ///     Contructor de la clase
        /// </summary>
        /// <param name="name">Nombre del campo</param>
        /// <param name="value">Valor del campo</param>
        public HttpPostedField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Obtiene el nombre del campo
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Obtiene el valor del campo
        /// </summary>
        public string Value { get; }

        #endregion
    }
}