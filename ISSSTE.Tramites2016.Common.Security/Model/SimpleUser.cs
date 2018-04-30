namespace ISSSTE.Tramites2016.Common.Security.Model
{
    /// <summary>
    ///     Representa la información mínima de un usuario que se puede proporcionar a clientes externos
    /// </summary>
    public class SimpleUser
    {
        /// <summary>
        ///     Obtiene o asigna el Id del usuario
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Obtiene o asigna ek numbre de usuario del usuario
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
    }
}