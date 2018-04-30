namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    ///     Contiene constantes utilizadas para hacer peticiones HTTP
    /// </summary>
    public static class HttpContants
    {
        /// <summary>
        ///     Contiene varios valores para el header Content-Type
        /// </summary>
        public static class ContentTypes
        {
            /// <summary>
            ///     Valor para contenido Json
            /// </summary>
            public const string Json = "application/json";

            /// <summary>
            ///     Valor para contenido de un formulario web
            /// </summary>
            public const string FormUrlEncode = "application/x-www-form-urlencoded";

            /// <summary>
            ///     Valor para contenido que se descarga como archivo
            /// </summary>
            public const string OctetStream = "application/octet-stream";

            /// <summary>
            ///     Valor para contenido que se descarga como pdf
            /// </summary>
            public const string Pdf = "application/pdf";
        }

        /// <summary>
        ///     Contiene varios valores para el header Content-Disposition
        /// </summary>
        public static class ContentDisposition
        {
            /// <summary>
            ///     Valor para los adjuntos
            /// </summary>
            public const string Attachment = "attachment";
        }

        /// <summary>
        ///     Contiene varios nombres de headers
        /// </summary>
        public static class Headers
        {
            /// <summary>
            ///     Contiene nombre y valores para el header Authorization
            /// </summary>
            public static class Authorization
            {
                /// <summary>
                ///     Nombre del header Authorization
                /// </summary>
                public const string Name = "Authorization";

                /// <summary>
                ///     Contiene varios valores que el header puede tener
                /// </summary>
                public static class Values
                {
                    /// <summary>
                    ///     Valor cuando se utiliza un Token OAuth 2.0
                    /// </summary>
                    public const string Bearer = "Bearer";
                }
            }
        }
    }
}