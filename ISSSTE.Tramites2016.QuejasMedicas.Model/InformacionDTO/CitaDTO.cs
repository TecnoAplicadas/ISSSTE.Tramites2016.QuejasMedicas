using System;
using System.Collections.Generic;
using System.Globalization;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class CitaDTO : DomicilioDTO
    {
        public CitaDTO()
        {
            CualquierEstado = false;
        }

        public bool CualquierEstado { get; set; }

        public int SolicitudId { get; set; }
        public int CatTipoTramiteId { get; set; }
        public int TramiteUnidadAtencionId { get; set; }
        public int UnidadMedicaId { get; set; }
        public int CatTipoEdoCitaId { get; set; } //MFP 15/03/2017

        public PersonaDTO Paciente { get; set; }
        public PersonaDTO Promovente { get; set; }

        public List<RequisitoDTO> Requisitos { get; set; }

        public DateTime FechaCita { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraCita { get; set; }



        public string CatTipoTramiteConcepto { get; set; }
        public string HoraInicio { get; set; }
        //public string FechaCitaVista => FormatoFechaCita();
        public string ConceptoEdoCita { get; set; }
        public string Unidad_Atencion { get; set; }
        public string Unidad_Medica { get; set; }
        public string NumeroFolio { get; set; }

        public string FechaConHora => FormatoHorario();
        public string FechaCitaVista => GetFecha();
        public string HoraCitaVista => GetHora();

        public string DomicilioUnidad { get; set; }

        public int? DomicilioId { get; set; }

        public bool EsRepresentante { get; set; }

        public bool ConVP { get; set; }
        public string ID_VP { get; set; }
        public bool ConFolio { get; set; }

        public string RespuestaCaptcha { get; set; }
        public string Estado { get; set; }

        private string FormatoFechaCita()
        {
            return FechaCita.ToString(CultureInfo.InvariantCulture);
        }

        private string FormatoHorario()
        {
            return GetFecha() + " " + GetHora();
        }

        private string GetHora()
        {
            if (HoraInicio == null) return "";
            var partesHora = HoraInicio.Split(':');

            return partesHora[0] + ":" + partesHora[1];
        }

        private string GetFecha()
        {
            return (Fecha.Day < 10 ? "0" + Fecha.Day : "" + Fecha.Day) + "/" +
                               (Fecha.Month < 10 ? "0" + Fecha.Month : "" + Fecha.Month) + "/" +
                               Fecha.Year ;
        }



    }
}