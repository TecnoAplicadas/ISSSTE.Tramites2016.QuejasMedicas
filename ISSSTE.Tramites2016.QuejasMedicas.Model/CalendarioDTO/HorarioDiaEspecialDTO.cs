using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO
{
    public class HorarioDiaEspecialDTO
    {
        public int DiaEspecialDTOScheduleId { get; set; }

        public int? RequestTypeId { get; set; }

        public int DelegationId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int Capacity { get; set; }

        public int Id { get; set; }

        public bool IsNonWorking { get; set; }

        public bool IsOverrided { get; set; }
    }
}