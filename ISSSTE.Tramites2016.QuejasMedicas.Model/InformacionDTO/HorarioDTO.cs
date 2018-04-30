using System;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class HorarioDTO : DoDisposeDTO
    {
        public string HoraInicioCad => HoraCalc(HoraInicio);
        public string FechaForma => FechaCalculada(Fecha);

        public bool EsFechaAcontecida { get; set; }
        public bool EsDiaNoLaboral { get; set; }
        public bool EsHoy { get; set; }
        public bool EsActivo { get; set; }
        public bool EsDiaDisponible { get; set; }

        public DateTime Fecha { get; set; }

        public int Capacidad { get; set; }
        public int HorarioId { get; set; }

        public TimeSpan HoraInicio { get; set; }

        private string FechaCalculada(DateTime Fecha)
        {
            return (Fecha.Day < 10 ? "0" + Fecha.Day : "" + Fecha.Day) + "/" +
                   (Fecha.Month < 10 ? "0" + Fecha.Month : "" + Fecha.Month) + "/" +
                   Fecha.Year;
        }

        private string HoraCalc(TimeSpan Tiempo)
        {
            dynamic cadena = Tiempo.ToString();
            cadena = cadena.Split(':');
            return cadena[0] + ":" + cadena[1];
        }
    }
}