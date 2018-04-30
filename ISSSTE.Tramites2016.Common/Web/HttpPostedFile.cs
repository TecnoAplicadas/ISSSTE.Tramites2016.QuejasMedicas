namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    ///     Representa una rchivo enviado como multipart
    /// </summary>
    public class HttpPostedFile
    {
        #region Constructor

        /// <summary>
        ///     Coontructor de la clase
        /// </summary>
        /// <param name="name">Nombre del archivo (en la petición)</param>
        /// <param name="filename">Nombre del archivo (en el sistema de archivos)</param>
        /// <param name="data">Contenido del archivo</param>
        public HttpPostedFile(string name, string filename, byte[] data)
        {
            Name = name;
            Filename = filename;
            Data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Obtiene el nombre del archivo (en la petición)
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Obtiene el nombre del archivo (en el sistema de archivos)
        /// </summary>
        public string Filename { get; }

        /// <summary>
        ///     Obtiene el contenido del archivo
        /// </summary>
        public byte[] Data { get; }

        #endregion
    }
}