using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO
{
    public class CalendarListDTO
    {
        /// <summary>
        ///     Lista de citas por dia de la semana y horarios
        /// </summary>
        public List<HorarioApiDTO> Schedules { get; set; }

        /// <summary>
        ///     Lista que contiene los dias no laborables
        /// </summary>
        public List<HorarioDiaEspecialDTO> NonLaborableDays { get; set; }

        /// <summary>
        ///     Lista que contiene los dias especiales con los horarios especificos
        /// </summary>
        public List<HorarioDiaEspecialApiDTO> SpecialSchedules { get; set; }
    }
}