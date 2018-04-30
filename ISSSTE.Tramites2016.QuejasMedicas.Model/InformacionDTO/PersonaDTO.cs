using System;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class PersonaDTO : DoDisposeDTO
    {
        public int PersonaId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string CURP { get; set; }
        public string Sexo => SexoCalculado(EsMasculino);

        public string Lada { get; set; }
        public string TelFijo { get; set; }
        public string TelMovil { get; set; }
        public string CorreoElectronico { get; set; }

        public bool EsMasculino { get; set; }

        public int Edad => EdadCalculada(FechaNacimiento);
        public int CatTipoTramiteId { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public bool? EsUsuarioBloqueado { get; set; }

        private int EdadCalculada(DateTime? Fecha)
        {
            if (Fecha == null) return 0;
            var nacimiento = (DateTime) Fecha;
            int edad = 0;

            int.TryParse((DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1).ToString(),out edad);

            return edad;
        }

        private string SexoCalculado(bool esMasculino)
        {
            return esMasculino ? Enumeracion.EnumVarios.Masculino : Enumeracion.EnumVarios.Femenino;
        }
    }
}