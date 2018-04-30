using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO
{
    public class HorarioDiaEspecialApiDTO
    {
        /// <summary>
        ///     Dia especial
        /// </summary>
        public HorarioDiaEspecialDTO DiaEspecialDTO { get; set; }

        /// <summary>
        ///     Lsita de citas por dia
        /// </summary>
        public List<HorarioDiaEspecialDTO> Schedules { get; set; }
    }
}