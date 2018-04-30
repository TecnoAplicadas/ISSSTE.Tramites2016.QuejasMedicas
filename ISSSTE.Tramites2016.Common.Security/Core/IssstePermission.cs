namespace ISSSTE.Tramites2016.Common.Security.Core
{
    /// <summary>
    ///     Representa los permisos que se tiene a un trámite
    /// </summary>
    public class IssstePermission
    {
        /// <summary>
        ///     Obtiene o asigna el valor que indica si se puede leer la información del trámite
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        ///     Obtiene o asigna el valor que indica si se puede craer un nuevo trámite
        /// </summary>
        public bool CanCreate { get; set; }

        /// <summary>
        ///     Obtiene o asigna el valor que indica si se puede editar un trámite existente
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        ///     Obtiene o asigna el valor que indica si se puede cancelar un trámite existente
        /// </summary>
        public bool CanCancel { get; set; }
    }
}