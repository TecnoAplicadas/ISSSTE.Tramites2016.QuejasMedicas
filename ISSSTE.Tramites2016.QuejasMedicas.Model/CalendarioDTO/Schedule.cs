using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public int? RequestTypeId { get; set; }

        public int DelegationId { get; set; }

        public int WeekdayId { get; set; }

        public TimeSpan Time { get; set; }

        public int Capacity { get; set; }
    }
}