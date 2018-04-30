#region

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Http;

#endregion

namespace ISSSTE.Tramites2016.Common.Web.Http
{
    /// <summary>
    ///     Forza la autorización de un controller o acción así como lo hace <see cref="AuthorizeAttribute" /> pero obteniendo
    ///     la lista de roles desde el app.config
    /// </summary>
    /// <remarks>
    ///     Solo puede ser utilizado en controladores de Web API, para utilizarlo en controladores de MVC, utilizar
    ///     <see cref="Mvc.AuthorizeByConfigAttribute" />
    /// </remarks>
    public class AuthorizeByConfigAttribute : AuthorizeAttribute, IAuthorizeByConfig
    {
        #region Fields

        /// <summary>
        ///     Almacena las llaves de donde obtener los roles
        /// </summary>
        private readonly string[] _roleKeys;

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor de la clase
        /// </summary>
        /// <param name="roleKeys">Nombre de la llave de donde obtener la lista de roles</param>
        public AuthorizeByConfigAttribute(params string[] roleKeys)
        {
            _roleKeys = roleKeys;

            var roles = GetRoles();

            Roles = string.Join(",", roles);
        }

        #endregion

        #region IAuthorizeByConfig Implementation

        /// <summary>
        ///     Obtiene los roles a partir de las llaves utilizadas en el contructor
        /// </summary>
        /// <returns>Lista de roles autorizados</returns>
        public List<string> GetRoles()
        {
            var roles = new List<string>();
            var allRoles = (NameValueCollection) ConfigurationManager.GetSection("authorizeRoles");

            foreach (var roleKey in _roleKeys)
                roles.AddRange(allRoles[roleKey].Split(',').Select(r => r.Trim()));

            return roles;
        }

        #endregion
    }
}