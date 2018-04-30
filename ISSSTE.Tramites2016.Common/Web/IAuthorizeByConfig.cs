using System.Collections.Generic;

namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    ///     Define métodos a exporner por atributos de autorización por configración
    /// </summary>
    public interface IAuthorizeByConfig
    {
        /// <summary>
        ///     Obtiene los roles a partir de las llaves utilizadas en el contructor
        /// </summary>
        /// <returns>Lista de roles autorizados</returns>
        List<string> GetRoles();
    }
}