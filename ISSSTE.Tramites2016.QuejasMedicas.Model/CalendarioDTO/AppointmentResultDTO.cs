using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO
{
    public class AppointmentResultDTO
    {
        public int Id { get; set; }
        public string NumeroFolio { get; set; }
        public string ColorTramite { get; set; }
        public int SolicitudId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Class { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan HoraCita { get; set; }
        public DateTime EndDate { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}