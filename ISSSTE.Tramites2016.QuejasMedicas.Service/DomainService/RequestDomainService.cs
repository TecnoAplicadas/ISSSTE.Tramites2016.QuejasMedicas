using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService
{
    public class RequestDomainService : DoDisposeDTO, ISolcitcitudService
    {
        public async Task<List<EstadoCitaDTO>> ObtenerStatusCita()
        {
            using (var context = new ISSSTEEntities())
            {
                var resuldato = await context.CatTipoEdoCita
                    .Select(S => new EstadoCitaDTO { Concepto = S.Concepto, EstadoCitaId = S.CatTipoEdoCitaId })
                    .ToListAsync();

                return resuldato;
            }
        }

        public async Task<bool> DesbloquearUsuarioPorSolicitudId(int RequestId)
        {
            using (var context = new ISSSTEEntities())
            {
                var result = await context.Solicitud
                    .Where(sol => sol.SolicitudId == RequestId)
                    .Join(context.Involucrado,
                        Req => Req.SolicitudId, Inv => Inv.SolicitudId,
                        (Req, Inv) => Inv)
                    .Join(context.Persona,
                        Inv => Inv.PersonaId, Per => Per.PersonaId,
                        (Inv, Per) => new { inv = Inv, per = Per })
                    .Where(S => S.inv.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente)
                    .FirstOrDefaultAsync();

                result.per.EsUsuarioBloqueado = false;
                result.per.FechaDesbloqueo = DateTime.Now;

                await context.SaveChangesAsync();

                return true;
            }
        }


        // PEER_AAAV_MARCELO
        public async Task<RequestResultDTO> GetRequestByIdAsync(int SolicitudId)
        {
            using (var context = new ISSSTEEntities())
            {

                var resultado = await context.Solicitud
                    .Join(context.TramiteUnidadAtencion,
                        Req => Req.TramiteUnidadAtencionId, Tua => Tua.TramiteUnidadAtencionId,
                        (Req, Tua) => new { req = Req, tua = Tua })
                    .Select(UaReq => new RequestResultDTO
                    {
                        SolicitudId = UaReq.req.SolicitudId,
                        Folio = UaReq.req.NumeroFolio,
                        CatTipoTramiteId = UaReq.tua.CatTipoTramite.CatTipoTramiteId,
                        CatTipoTramiteConcepto = UaReq.tua.CatTipoTramite.Concepto,
                        Cita = new CitaDTO
                        {
                            CatTipoEdoCitaId = UaReq.req.CatTipoEdoCitaId,
                            FechaCita = UaReq.req.FechaCita,
                            HoraCita = UaReq.req.Horario.HoraInicio,
                            ConceptoEdoCita = UaReq.req.CatTipoEdoCita.Concepto
                        },
                        Paciente = UaReq.req.Involucrado
                            .Where(S => S.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente)
                            .Select(S => new PersonaDTO
                            {
                                PersonaId = S.PersonaId,
                                CURP = S.Persona.CURP,
                                Materno = S.Persona.Materno,
                                Paterno = S.Persona.Paterno,
                                FechaNacimiento = S.Persona.FechaNacimiento,
                                Nombre = S.Persona.Nombre,
                                EsMasculino = S.Persona.EsMasculino,
                                TelFijo = S.Persona.Telefono,
                                TelMovil = S.Persona.TelefonoMovil,
                                CorreoElectronico = S.Persona.Correo,
                                EsUsuarioBloqueado=S.Persona.EsUsuarioBloqueado
                            })
                            .FirstOrDefault(),
                        Promovente = UaReq.req.Involucrado
                            .Where(S => S.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Promovente)
                            .Select(S => new PersonaDTO
                            {
                                PersonaId = S.PersonaId,
                                CURP = S.Persona.CURP,
                                Materno = S.Persona.Materno,
                                Paterno = S.Persona.Paterno,
                                FechaNacimiento = S.Persona.FechaNacimiento,
                                Nombre = S.Persona.Nombre,
                                EsMasculino = S.Persona.EsMasculino,
                                TelFijo = S.Persona.Telefono,
                                TelMovil = S.Persona.TelefonoMovil,
                                CorreoElectronico = S.Persona.Correo,
                                EsUsuarioBloqueado=S.Persona.EsUsuarioBloqueado
                            })
                            .FirstOrDefault(),
                        UnidadAtencion = new UnidadAtencionDTO
                        {
                            UnidadAtencionId = UaReq.tua.UnidadAtencionId,
                            Descripcion = UaReq.tua.UnidadAtencion.Descripcion
                        }
                    })
                    .Where(S => S.SolicitudId == SolicitudId)
                    .FirstOrDefaultAsync();

                return resultado;

            }
        }

        //public bool EsUsuarioBloqueado(int CatTipoTramiteId)
        //{
        //    using (var context = new ISSSTEEntities())
        //    {
        //        var result = context.Involucrado
        //            .Where(inv => inv.EsUsuarioBloqueado == true
        //            && inv.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente
        //            && inv.Solicitud.TramiteUnidadAtencion.CatTipoTramiteId == CatTipoTramiteId);

        //        return result.Any();
        //    }

        //}


        /// <summary>
        ///     Obtiene una lista de solicitudes por diversos parámetros
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="Page"></param>
        /// <param name="RoleName"></param>
        /// <param name="DelegationId"></param>
        /// <param name="Username"></param>
        /// <param name="Dates"></param>
        /// <param name="StatusId"></param>
        /// <param name="Query"></param>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        // PEER_AAAV_MARCELO
        public async Task<PagedInformationDTO<RequestResultDTO>> ObtenerSolicituesPaginadasAsync(List<int> DelegationId,
            DatesDTO Fechas, int PageSize, int Page, int? StatusId = default(int?), string Query = null, int? CatTipoTramiteId = null)
        {
            var pageInfo = new PagedInformationDTO<RequestResultDTO>
            {
                SetElementosPorPagina = PageSize,
                CurrentPage = Page,
                QueryString = Query
            };

            using (var context = new ISSSTEEntities())
            {
                var requestsQuery = context.Solicitud
                    .Join(context.TramiteUnidadAtencion,
                        Req => Req.TramiteUnidadAtencionId, Tua => Tua.TramiteUnidadAtencionId,
                        (Req, Tua) => new { req = Req, tua = Tua })
                    .Select(UaReq => new RequestResultDTO
                    {
                        SolicitudId = UaReq.req.SolicitudId,
                        CatTipoTramiteId = UaReq.tua.CatTipoTramiteId,
                        CatTipoTramiteConcepto = UaReq.tua.CatTipoTramite.Concepto,
                        Folio = UaReq.req.NumeroFolio,
                        Cita = new CitaDTO
                        {
                            CatTipoEdoCitaId = UaReq.req.CatTipoEdoCitaId,
                            FechaCita = UaReq.req.FechaCita,
                            HoraCita = UaReq.req.Horario.HoraInicio,
                            ConceptoEdoCita = UaReq.req.CatTipoEdoCita.Concepto
                        },
                        Paciente = UaReq.req.Involucrado
                                .Where(S => S.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente)
                                .Select(S => new PersonaDTO
                                {
                                    PersonaId = S.PersonaId,
                                    CURP = S.Persona.CURP,
                                    Materno = S.Persona.Materno,
                                    Paterno = S.Persona.Paterno,
                                    FechaNacimiento = S.Persona.FechaNacimiento,
                                    Nombre = S.Persona.Nombre,
                                    EsMasculino = S.Persona.EsMasculino,
                                    CorreoElectronico = S.Persona.Correo,
                                    TelFijo = "(" + S.Persona.Lada + ") " + S.Persona.Telefono,
                                    TelMovil = S.Persona.TelefonoMovil
                                })
                                .FirstOrDefault(),
                        Promovente = UaReq.req.Involucrado
                                .Where(S => S.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Promovente)
                                .Select(S => new PersonaDTO
                                {
                                    PersonaId = S.PersonaId,
                                    CURP = S.Persona.CURP,
                                    Materno = S.Persona.Materno,
                                    Paterno = S.Persona.Paterno,
                                    FechaNacimiento = S.Persona.FechaNacimiento,
                                    Nombre = S.Persona.Nombre,
                                    EsMasculino = S.Persona.EsMasculino,
                                    CorreoElectronico = S.Persona.Correo,
                                    TelFijo = "(" + S.Persona.Lada + ") " + S.Persona.Telefono,
                                    TelMovil = S.Persona.TelefonoMovil
                                })
                                .FirstOrDefault(),
                        UnidadAtencion = new UnidadAtencionDTO
                        {
                            UnidadAtencionId = UaReq.tua.UnidadAtencionId,
                            Descripcion = UaReq.tua.UnidadAtencion.Descripcion
                        }
                    }
                    );

                if(Fechas.FechaInicio.HasValue && Fechas.FechaFin.HasValue)
                requestsQuery = requestsQuery.Where(R => R.Cita.FechaCita >= Fechas.FechaInicio
                                                         && R.Cita.FechaCita <= Fechas.FechaFin);

                if (!DelegationId.Contains(Enumeracion.EnumVarios.TodasLasDelegaciones))
                    requestsQuery = requestsQuery
                        .Where(R => DelegationId.Contains(R.UnidadAtencion.UnidadAtencionId));

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery.Where(
                        R => R.Folio.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                             || R.Paciente.Nombre.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                             || R.Promovente.Nombre.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                   );

                if (StatusId.HasValue)
                    requestsQuery = requestsQuery.Where(R => R.Cita.CatTipoEdoCitaId == StatusId);

                if (CatTipoTramiteId.HasValue)
                    requestsQuery = requestsQuery.Where(R => R.CatTipoTramiteId == CatTipoTramiteId);

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.Cita.FechaCita)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();

                pageInfo.ResultList = requests;
            }

            return pageInfo;
        }


        public async Task<int> ActulizarEstadoCita(int SolicitudId, int StatusId)
        {
            using (var context = new ISSSTEEntities())
            {
                var result = await context.Solicitud
                    .Where(S => S.SolicitudId == SolicitudId)
                    .FirstOrDefaultAsync();

                result.CatTipoEdoCitaId = StatusId;

                return await context.SaveChangesAsync();
            }
        }


        /// <summary>
        ///     Obtiene el detalle de la solicitud por Id
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <returns></returns>
        public RequestResultDTO GetRequestById(int SolicitudId)
        {
            using (var context = new ISSSTEEntities())
            {
                var result = context.Solicitud
                    .Join(context.TramiteUnidadAtencion,
                        Req => Req.TramiteUnidadAtencionId, Tua => Tua.TramiteUnidadAtencionId,
                        (Req, Tua) => new { req = Req, tua = Tua }
                    )
                    .Select(UaReq => new RequestResultDTO
                    {
                        SolicitudId = UaReq.req.SolicitudId,
                        Folio = UaReq.req.NumeroFolio,
                        Cita = new CitaDTO
                        {
                            CatTipoEdoCitaId = UaReq.req.CatTipoEdoCitaId,
                            FechaCita = UaReq.req.FechaCita,
                            Fecha = UaReq.req.FechaCita,
                            HoraCita = UaReq.req.Horario.HoraInicio,
                            HoraInicio = UaReq.req.Horario.HoraInicio.ToString(),
                            ConceptoEdoCita = UaReq.req.CatTipoEdoCita.Concepto
                        },
                        Paciente = UaReq.req.Involucrado
                            .Where(S => S.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente)
                            .Select(S => new PersonaDTO
                            {
                                PersonaId = S.PersonaId,
                                CURP = S.Persona.CURP,
                                Materno = S.Persona.Materno,
                                Paterno = S.Persona.Paterno,
                                FechaNacimiento = S.Persona.FechaNacimiento,
                                Nombre = S.Persona.Nombre,
                                EsMasculino = S.Persona.EsMasculino,
                                TelFijo = S.Persona.Telefono,
                                TelMovil = S.Persona.TelefonoMovil,
                                CorreoElectronico = S.Persona.Correo,
                                EsUsuarioBloqueado = S.Persona.EsUsuarioBloqueado
                            })
                            .FirstOrDefault(),
                        Promovente = UaReq.req.Involucrado
                            .Where(S => S.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Promovente)
                            .Select(S => new PersonaDTO
                            {
                                PersonaId = S.PersonaId,
                                CURP = S.Persona.CURP,
                                Materno = S.Persona.Materno,
                                Paterno = S.Persona.Paterno,
                                FechaNacimiento = S.Persona.FechaNacimiento,
                                Nombre = S.Persona.Nombre,
                                EsMasculino = S.Persona.EsMasculino,
                                TelFijo = S.Persona.Telefono,
                                TelMovil = S.Persona.TelefonoMovil,
                                CorreoElectronico = S.Persona.Correo,
                                EsUsuarioBloqueado = S.Persona.EsUsuarioBloqueado
                            })
                            .FirstOrDefault(),
                        UnidadAtencion = new UnidadAtencionDTO
                        {
                            UnidadAtencionId = UaReq.tua.UnidadAtencionId,
                            Descripcion = UaReq.tua.UnidadAtencion.Descripcion
                        }
                    })
                    .FirstOrDefault(S => S.SolicitudId == SolicitudId);

                return result;
            }
        }
    }
}