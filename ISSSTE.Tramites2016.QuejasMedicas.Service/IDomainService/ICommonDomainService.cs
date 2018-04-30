using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.ComunDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService
{
    public interface ICommonDomainService
    {
        Task<NotificacionDTO> ObtenerNotificacionesPorUADyCSAsync(DatesDTO FechasConsulta, int[] UADyCSIds);

        Task<IEnumerable<DiaSemanaDTO>> ObtenerDiasSemanaAsync();

        Task<IEnumerable<DelegacionDTO>> ObtenerUADyCSAsync();

        Task<IEnumerable<DelegacionDTO>> ObtenerUADyCSPorUADyCSIdsAsync(int[] UADyCSIds);

        Task<IEnumerable<DelegacionDTO>> GetDelegationsByConfig(int CatTipoTramiteId, int[] UADyCSIds);

        Task<List<TramiteDTO>> ObtenerTiposDeTramiteAsync();

        Task<List<CatTipoEntidadDTO>> ObtenerEntidadesFederativasAsync();

        Task<List<EstadoCitaDTO>> ObtenerEStatusSiguientesAsync();
    }
}