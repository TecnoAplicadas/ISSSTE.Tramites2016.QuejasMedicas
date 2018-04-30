using System;

namespace ISSSTE.Tramites2016.Common.Security.Claims
{
    /// <summary>
    /// Contiene URIs de los tipos de claims utilizador por el sistema de deguridad del ISSSTE
    /// </summary>
    public class IsssteTramitesClaimTypes
    {
        /// <summary>
        /// El URI para un claim que especifica el token de acceso de un usuario, http://schemas.issste.gob.mx/ws/2014/11/identity/claims/access_token.
        /// </summary>
        public const string AccessToken = "http://schemas.issste.gob.mx/ws/2014/11/identity/claims/access_token";

        /// <summary>
        /// El URI para un claim que especifica el refresh token de un usuario, http://schemas.issste.gob.mx/ws/2014/11/identity/claims/refresh_token.
        /// </summary>
        public const string RefreshToken = "http://schemas.issste.gob.mx/ws/2014/11/identity/claims/refresh_token";

        /// <summary>
        /// El URI para un claim que especifica las propiedades de un usuario, http://schemas.issste.gob.mx/ws/2015/08/identity/claims/external_properties.
        /// </summary>
        public const string Properties = "http://schemas.issste.gob.mx/ws/2015/08/identity/claims/external_properties";

        /// <summary>
        /// El URI para un claim que especifica loss roles de un usuario, http://schemas.issste.gob.mx/ws/2014/11/identity/claims/external_roles.
        /// </summary>
        public const string Roles = "http://schemas.issste.gob.mx/ws/2014/11/identity/claims/external_roles";
    }
}