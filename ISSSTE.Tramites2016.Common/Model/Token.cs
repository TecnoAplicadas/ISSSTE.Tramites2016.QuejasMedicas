namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    ///     Token a usarse en las peticiones
    /// </summary>
    public class Token
    {
        /// <summary>
        ///     Token cifrado
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        ///     Tipo de token
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        ///     Fecha de vigencia
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        ///     Usuario del token
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        ///     Is used
        /// </summary>
        public string issued { get; set; }

        /// <summary>
        ///     Fecha de vencimiento
        /// </summary>
        public string expires { get; set; }
    }
}