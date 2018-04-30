using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService
{
    public interface ICalendarDomainService
    {
        Task<CalendarDTO> GetAppointments(List<int> Delegations, DatesDTO Fechas, string Query = null,
            int? StatusId = null, int? SolicitudId = null);

        Task<List<HorarioDiaEspecialDTO>> ObtenerDiasNoLaborablesAsync(int? SolicitudId, int UnidadAtencionId);

        Task<List<HorarioApiDTO>> ObtenerHorariosAsync(int? SolicitudId, int UnidadAtencionId);

        Task<List<HorarioDiaEspecialApiDTO>> ObtenerDiasEspecialesAsync(int? SolicitudId, int UnidadAtencionId);

        Task<int> GuardarHorariosAsync(List<Schedule> Saves);

        Task<int> GuardarHorariosDiasEspecialesAsync(List<HorarioDiaEspecialDTO> SpecialSchedulesn);

        Task<int> GuardaDiasNoLaboralesOEspecialesAsync(List<HorarioDiaEspecialDTO> SpecialDays);

        Task<int> DeleteNonLaboraleOEspecialDaysAsync(HorarioDiaEspecialDTO SpecialDay);

        Task<int> EliminarHorarios(int? RequestTypeId, int UnidadAtencionId, TimeSpan HorarioInicio,
            int? CatTipoDiaSemanaId, bool EsDiaEspecial);
    }
}