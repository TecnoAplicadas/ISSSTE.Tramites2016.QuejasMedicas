using System;
using System.Collections.Generic;
using System.Linq;

namespace ISSSTE.Tramites2016.Common.Security.Core
{
    /// <summary>
    ///     Representa la información de un usuario
    /// </summary>
    public class IsssteUser
    {
        #region Constants

        /// <summary>
        ///     Valor de la delegación que representa que se puede acceder a la información todas las delegaciones
        /// </summary>
        private readonly int AllDelegationsId = -1;

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor del usuario
        /// </summary>
        public IsssteUser()
        {
            Properties = new List<IsssteUserProperty>();
            Roles = new List<IsssteRole>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Obtiene o asigna Id del usuario
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Obtiene o asigna el nombre de usuario del usuario
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Obtiene o asigna el nombre del usuario
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Obtiene o asigna el correo electrónico del usuario
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Obtiene o asigna la lista de propiedades del usuario
        /// </summary>
        public List<IsssteUserProperty> Properties { get; set; }

        /// <summary>
        ///     Obtiene o asigna la lista de rolew del usuario
        /// </summary>
        public List<IsssteRole> Roles { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Obtiene los posibles valore que un usuario tiene asignado bajo un nombre de una propiedad
        /// </summary>
        /// <param name="PropertyName">Nombre de la propiedad</param>
        /// <returns>Lista de valores que el usuario tiene asignado</returns>
        public List<IsssteUserProperty> GetPropertyValues(string PropertyName)
        {
            var properties = Properties?.Where(P => P.Type == PropertyName).Select(P => P).ToList() ??
                             new List<IsssteUserProperty>();

            return properties;
        }

        /// <summary>
        ///     Obtiene la lista de ids de delegaciones que el usuario tiene asignadas
        /// </summary>
        /// <returns>Lista de ids de delegaciones</returns>
        public List<int> GetDelegations()
        {
            return GetPropertyValues(IsssteUserPropertyTypes.Delegation).Select(P => Convert.ToInt32(P.Value)).ToList();
        }

        public List<int> GetUADyCS()
        {
            return GetPropertyValues(IsssteUserPropertyTypes.UADyCS).Select(P => Convert.ToInt32(P.Value)).ToList();
        }

        /// <summary>
        ///     Valida si el usuario tiene permiso para acceder a la información de todas las delegaciones
        /// </summary>
        /// <returns>Resultado de la validación</returns>
        public bool HasAuthorizationToAllDelegations()
        {
            return GetDelegations().Any(D => D == AllDelegationsId);
        }

        #endregion
    }
}