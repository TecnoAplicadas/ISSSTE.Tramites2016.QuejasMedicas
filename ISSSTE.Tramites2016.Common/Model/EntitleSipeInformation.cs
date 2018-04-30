#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de un derechohabiente
    /// </summary>
    public class EntitleSipeInformation
    {
        /// <summary>
        ///     Numero de ISSSTE del derechohabiente
        /// </summary>
        public String NumIssste { get; set; }

        /// <summary>
        ///     Apellido paterno del derechohabiente
        /// </summary>
        public String FirstSurname { get; set; }

        /// <summary>
        ///     Apellido materno del derechohabiente
        /// </summary>
        public String SecondSurname { get; set; }

        /// <summary>
        ///     Nombre del derechohabiente
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Clave del lugar de nacimiento
        /// </summary>
        public String EntityBirthKey { get; set; }

        /// <summary>
        ///     Fecha de nacimiento
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        ///     RFC del derechohabiente
        /// </summary>
        public String Rfc { get; set; }

        /// <summary>
        ///     CURP del derechohabiente
        /// </summary>
        public String Curp { get; set; }

        /// <summary>
        ///     Genero: Masculino o femenino
        /// </summary>
        public String Genger { get; set; }

        /// <summary>
        ///     Edad del beneficiario
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        ///     Estado civil
        /// </summary>
        public String MaritalStatus { get; set; }

        /// <summary>
        ///     Dirección. Calle registrada
        /// </summary>
        public String Street { get; set; }

        /// <summary>
        ///     Numero exterior
        /// </summary>
        public String ExteriorNumber { get; set; }

        /// <summary>
        ///     Numero interior
        /// </summary>
        public String InteriorNumber { get; set; }

        /// <summary>
        ///     Codigo postal
        /// </summary>
        public String PostalCode { get; set; }

        /// <summary>
        ///     Dirección. Colonia registrada
        /// </summary>
        public String Colony { get; set; }

        /// <summary>
        ///     Numero telefonico
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        ///     Población
        /// </summary>
        public String Population { get; set; }

        /// <summary>
        ///     Estado
        /// </summary>
        public String State { get; set; }

        /// <summary>
        ///     Descripción de estado
        /// </summary>
        public String StateDescription { get; set; }

        /// <summary>
        ///     Clave de colonia
        /// </summary>
        public String ColonyKey { get; set; }

        /// <summary>
        ///     Clave de estadoc civil
        /// </summary>
        public String MaritalStatusKey { get; set; }

        /// <summary>
        ///     Tipo de régimen registrado
        /// </summary>
        public string RegimeType { get; set; }

        /// <summary>
        ///     Lugar de nacimiento
        /// </summary>
        public String EntityBirth { get; set; }

        /// <summary>
        ///     Id de la delegación ISSSTE a la que pertence
        /// </summary>
        public string DelegationCode { get; set; }

        /// <summary>
        ///     Telefono registrado
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        ///     Correo electrónico registrado
        /// </summary>
        public string Email { get; set; }


        //CAP
        public string DirectType { get; set; }
    }
}