using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService
{
    public interface ISolcitcitudService
    {
        /// <summary>
        /// Obtiene la lista de estados de las citas
        /// </summary>
        /// <returns></returns>
        Task<List<EstadoCitaDTO>> ObtenerStatusCita();

        /// <summary>
        /// Actualiza el estado de la cita por el SolicitudId
        /// </summary>
        /// <param name="AppointmentId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        Task<int> ActulizarEstadoCita(int AppointmentId, int StatusId);


        /// <summary>
        /// Obtiene una lista de solicitudes paginada de acuerdo a los 
        /// criterios de busqueda pasados como parametros
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="DelegationId"></param>
        /// <param name="Username"></param>
        /// <param name="Dates"></param>
        /// <param name="PageSize"></param>
        /// <param name="Page"></param>
        /// <param name="StatusId"></param>
        /// <param name="Query"></param>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        Task<PagedInformationDTO<RequestResultDTO>> ObtenerSolicituesPaginadasAsync
        (List<int> DelegationId, DatesDTO Fechas, int PageSize,int Page, int? StatusId = default(int?), string Query = null, int? CatTipoTramiteId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <returns></returns>
        Task<RequestResultDTO> GetRequestByIdAsync(int SolicitudId);

        /// <summary>
        /// Desbloquear usuario por SolicitudId
        /// </summary>
        /// <param name="RequestId"></param>
        /// <returns></returns>
        Task<bool> DesbloquearUsuarioPorSolicitudId(int RequestId);
    }
}