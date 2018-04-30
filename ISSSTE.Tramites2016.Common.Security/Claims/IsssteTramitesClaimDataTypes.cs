namespace ISSSTE.Tramites2016.Common.Security.Claims
{
    /// <summary>
    /// Contiene URIs de los tipos de dato de claims utilizador por el sistema de deguridad del ISSSTE
    /// </summary>
    public class IsssteTramitesClaimDataTypes
    {
        /// <summary>
        /// El URI para el tipo de dato de un claim que contiene propiedades de un usuario, http://www.issste.org/2015/XMLSchema#Properties.
        /// </summary>
        public const string Properties = "http://www.issste.org/2015/XMLSchema#Properties";

        /// <summary>
        /// El URI para el tipo de dato de un claim que contiene roles de un usuario, http://www.issste.org/2014/XMLSchema#Role.
        /// </summary>
        public const string Roles = "http://www.issste.org/2014/XMLSchema#Role";
    }
}