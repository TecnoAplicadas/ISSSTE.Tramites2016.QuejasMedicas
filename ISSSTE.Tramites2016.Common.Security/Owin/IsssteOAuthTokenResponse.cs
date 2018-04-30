using System;

namespace ISSSTE.Tramites2016.Common.Security.Owin
{
    /// <summary>
    /// Representa la respuesta a una solicitud de token OAuth 2.0
    /// </summary>
    internal class IsssteOAuthTokenResponse
    {
        /// <summary>
        /// Obtiene o asigna el tipo de token
        /// </summary>
        public string Token_Type { get; set; }

        /// <summary>
        /// Obtiene o asigna el token de acceso
        /// </summary>
        public string Access_Token { get; set; }

        /// <summary>
        /// Obtiene o asigna el tiempo de expiración del token
        /// </summary>
        public int Expires_In { get; set; }

        /// <summary>
        /// Obtiene o asigna el refresh token
        /// </summary>
        public string Refresh_Token { get; set; }

        /// <summary>
        /// Obtiene o asigna el id del cliente que solicito el token
        /// </summary>
        public string Client_Id { get; set; }

        /// <summary>
        /// Obtiene o asigna el id del usuario que solicito el token
        /// </summary>
        public string User_Id { get; set; }

        /// <summary>
        /// Obtiene o asigna el nombre de usuario de usuario que solicito el token
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// Obtiene o asigna el correo electrónico del usuario que solicito el token
        /// </summary>
        public string Email { get; set; }
    }
}