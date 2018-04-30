using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO
{
    public class HorarioApiDTO
    {
        /// <summary>
        ///     Id de la cita
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        ///     /Id de la Delegación
        /// </summary>
        public int DelegationId { get; set; }

        /// <summary>
        ///     Id del dia de la semana
        /// </summary>
        public int WeekdayId { get; set; }

        /// <summary>
        ///     Hora de la cita
        /// </summary>
        public TimeSpan? Time { get; set; }

        /// <summary>
        ///     Capacidad de la cita
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        ///     Nombre del dia de la semana
        /// </summary>
        public string WeekDay { get; set; }
    }
}