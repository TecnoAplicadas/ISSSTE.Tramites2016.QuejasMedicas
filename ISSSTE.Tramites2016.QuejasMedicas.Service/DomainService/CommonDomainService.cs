using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.ComunDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService
{
    public class CommonDomainService : DoDisposeDTO, ICommonDomainService
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fechas"></param>
        /// <param name="UADyCSIds"></param>
        /// <returns></returns>
        public async Task<NotificacionDTO> ObtenerNotificacionesPorUADyCSAsync(DatesDTO Fechas, int[] UADyCSIds)
        {
            using (var context = new ISSSTEEntities())
            {
                //PEER -> Debe levar esta inicializacion para evitar un error al hacer el count.
                List<CitaDTO> resultado = new List<CitaDTO>();

                if (UADyCSIds.Contains(Enumeracion.EnumVarios.TodasLasDelegaciones)) return default(NotificacionDTO);

                resultado = await context.Solicitud
                    .Join(context.TramiteUnidadAtencion,
                    Req => Req.TramiteUnidadAtencionId, Tua => Tua.TramiteUnidadAtencionId,
                    (Req, Tua) => new { Solicitudes = Req, TramiteUnidadAtencion = Tua })
                    .Where(R => R.Solicitudes.EsActivo && R.Solicitudes.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.Pendiente &&
                            R.Solicitudes.FechaCita >= Fechas.FechaInicio && R.Solicitudes.FechaCita <= Fechas.FechaFin
                            && UADyCSIds.Contains(R.TramiteUnidadAtencion.UnidadAtencionId)
                       )
                     .Select(S => new CitaDTO
                     {
                         SolicitudId = S.Solicitudes.SolicitudId,
                         NumeroFolio = S.Solicitudes.NumeroFolio,
                         FechaCita = S.Solicitudes.FechaCita,
                         HoraCita = S.Solicitudes.Horario.HoraInicio,
                         CatTipoTramiteConcepto = S.TramiteUnidadAtencion.CatTipoTramite.Concepto
                     })
                    .ToListAsync();

                return new NotificacionDTO
                {
                    ListaCitasPendientes = resultado,
                    NumeroCitasPendientes = resultado.Count
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DiaSemanaDTO>> ObtenerDiasSemanaAsync()
        {
            using (var context = new ISSSTEEntities())
            {
                var resultado = await context.CatTipoDiaSemana
                    .Where(S => S.EsActivo)
                    .Select(S => new DiaSemanaDTO { CatTipoDiaSemanaId = S.CatTipoDiaSemanaId, Dia = S.Dia })
                    .ToListAsync();

                return resultado;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequestTypeId"></param>
        /// <param name="DelegationsIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DelegacionDTO>> GetDelegationsByConfig(int RequestTypeId, int[] DelegationsIds)
        {
            List<DelegacionDTO> delegations = new List<DelegacionDTO>();

            using (var context = new ISSSTEEntities())
            {
                var delegationResult = context.TramiteUnidadAtencion
                    .Where(S => S.EsActivo && S.CatTipoTramiteId == RequestTypeId)
                    .AsQueryable();

                if (!DelegationsIds.Contains(-1))
                    delegationResult = delegationResult.Where(D => DelegationsIds.Contains(D.UnidadAtencionId));

                var result = delegationResult.Select(S => S.UnidadAtencion);

                delegations = await result
                    .Select(D => new DelegacionDTO { DelegacionId = D.UnidadAtencionId, Descripcion = D.Descripcion })
                    .ToListAsync();
            }

            return delegations;
        }

        /// <summary>
        ///     Obtiene las delegaciones del issste del usuario firmado
        /// </summary>
        /// <param name="DelegationsIds"></param>
        /// <returns>Una lista de delegaciones del issste</returns>
        public async Task<IEnumerable<DelegacionDTO>> ObtenerUADyCSPorUADyCSIdsAsync(int[] DelegationsIds)
        {
            List<DelegacionDTO> delegations = new List<DelegacionDTO>();

            using (var context = new ISSSTEEntities())
            {
                var delegationResult = context.UnidadAtencion.AsQueryable();

                if (!DelegationsIds.Contains(-1))
                    delegationResult = delegationResult.Where(D => DelegationsIds.Contains(D.UnidadAtencionId));

                delegations = await delegationResult
                    .Select(D => new DelegacionDTO { DelegacionId = D.UnidadAtencionId, Descripcion = D.Descripcion })
                    .ToListAsync();
            }

            return delegations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CatTipoEntidadDTO>> ObtenerEntidadesFederativasAsync()
        {
            List<CatTipoEntidadDTO> entidades = new List<CatTipoEntidadDTO>();

            using (var context = new ISSSTEEntities())
            {
                entidades = await context.CatTipoEntidad.Select(S => new CatTipoEntidadDTO
                {
                    CatTipoEntidadId = S.CatTipoEntidadId,
                    Concepto = S.Concepto,
                    EsActivo = S.EsActivo
                })
                    .ToListAsync();
            }

            return entidades;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TramiteDTO>> ObtenerTiposDeTramiteAsync()
        {
            List<TramiteDTO> tramite = new List<TramiteDTO>();

            using (var context = new ISSSTEEntities())
            {
                tramite = await context.CatTipoTramite
                    .Where(S => S.EsActivo)
                    .Select(S => new TramiteDTO
                    {
                        CatTipoTramiteId = S.CatTipoTramiteId,
                        Concepto = S.Concepto,
                        Semaforo = S.Semaforo,
                        EsActivo = S.EsActivo
                    })
                    .ToListAsync();
            }

            return tramite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<EstadoCitaDTO>> ObtenerEStatusSiguientesAsync()
        {
            List<EstadoCitaDTO> estadosCitas = new List<EstadoCitaDTO>();

            using (var context = new ISSSTEEntities())
            {
                estadosCitas = await context.CatTipoEdoCita
                    .Where(S =>
                        S.CatTipoEdoCitaId != Enumeracion.EnumTipoEdoCita.Pendiente &&
                        S.CatTipoEdoCitaId != Enumeracion.EnumTipoEdoCita.Cancelado
                        && S.EsActivo ==true
                    )
                    .Select(S => new EstadoCitaDTO { Concepto = S.Concepto, EstadoCitaId = S.CatTipoEdoCitaId })
                    .ToListAsync();
            }
            return estadosCitas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DelegacionDTO>> ObtenerUADyCSAsync()
        {
            List<DelegacionDTO> delegations = new List<DelegacionDTO>();

            using (var context = new ISSSTEEntities())
            {
                delegations = await context.UnidadAtencion
                    .Select(D => new DelegacionDTO { DelegacionId = D.UnidadAtencionId, Descripcion = D.Descripcion })
                    .ToListAsync();
            }
            return delegations;
        }
    }
}