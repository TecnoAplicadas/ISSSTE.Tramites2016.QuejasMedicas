using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using static ISSSTE.Tramites2016.QuejasMedicas.Model.Enums.Enumeracion;
using ISSSTE.Tramites2016.QuejasMedicas.Helpers;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.Common.Mail;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Common.Web.Http;
using ISSSTE.Tramites2016.Common.Security.Helpers;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    [AuthorizeByConfig("AllAdminRoles")]
    [RoutePrefix("api/Administrator")]
    public class AdministratorApiController : BaseApiController
    {
        /// <summary>
        ///     Administracion de calendario
        /// </summary>
        private readonly ICalendarDomainService _calendarDomainService;

        /// <summary>
        ///     Configuraciones de la aplicacion
        /// </summary>
        private readonly IConfiguracionesService _configuracionesService;

        private readonly IMailService _mailService;

        /// <summary>
        ///     Administracion de solicitudes
        /// </summary>
        private readonly ISolcitcitudService _requestDomainService;

        public AdministratorApiController(ISolcitcitudService RequestDomainService,
            ICalendarDomainService CalendarDomainService,
            IConfiguracionesService ConfiguracionesService,
            IMailService MailService
        )
        {
            _requestDomainService = RequestDomainService;
            _calendarDomainService = CalendarDomainService;
            _configuracionesService = ConfiguracionesService;
            _mailService = MailService;
        }


        /// Se utiliza para validar que el token de autenticación sigue siendo válido
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(void))]
        [HttpGet]
        [Route("Token/Validate")]
        public HttpResponseMessage ValidateLogin()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<string>))]
        [HttpGet]
        [Route("User/Roles")]
        public HttpResponseMessage GetUserRoles()
        {
            return HandleOperationExecution(() =>
            {
                var owinContext = Request.GetOwinContext();
                var rolesNames = new List<string>();

                var roles = owinContext.GetAuthenticatedUserRoles();

                //if (roles != null)
                rolesNames = roles.Select(R => R.Name).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, rolesNames);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(PagedInformationDTO<RequestResultDTO>))]
        [HttpGet]
        [Route("Requests")]
        public async Task<HttpResponseMessage> GetRequests(int pageSize, int page, string dates, string query = null,
            int? statusId = null, int? delegationId = null, int? RequestTypeId = null)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();

                var user = owinContext.GetAuthenticatedUser();

                var delegations = new List<int>();

                if (delegationId != null)
                {
                    delegations.Add(Convert.ToUInt16(delegationId));
                    if (!user.GetUADyCS().Contains(EnumVarios.TodasLasDelegaciones))
                        delegations = user.GetUADyCS().Intersect(delegations).ToList();
                }
                else
                {
                    delegations = user.GetUADyCS();
                }

                var fechas = FechaConversion.GetDates(dates);

                var result =
                    await
                        _requestDomainService.ObtenerSolicituesPaginadasAsync(delegations, fechas, pageSize,
                            page, statusId, query, RequestTypeId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Ontiene la informacion de la solicitud pasada como parametro
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(RequestResultDTO))]
        [HttpGet]
        [Route("AllInformation/{requestId}")]
        public async Task<HttpResponseMessage> GetRequestById(int requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _requestDomainService.GetRequestByIdAsync(requestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(PagedInformationDTO<RequisitoDTO>))]
        [HttpPost]
        [Route("requirements")]
        public async Task<HttpResponseMessage> GetRequirements(ReqPeticionDTO Reqst)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _configuracionesService.ConsultaRequisitos(Reqst.Paginado, null, Reqst.RequestTypeId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(PagedInformationDTO<CatalogoResultDTO>))]
        [HttpPost]
        [Route("Settings")]
        public async Task<HttpResponseMessage> GetSettings(BasePageInformationDTO Paginado)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _configuracionesService.ConsultaCatSysConfiguracion(Paginado, true);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SaveRequirementDetail")]
        public async Task<HttpResponseMessage> SaveRequirementDetail(RequisitoDTO SettingDetail)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _configuracionesService.SaveRequirementDetail(SettingDetail);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SaveSettingDetail")]
        public async Task<HttpResponseMessage> SaveSettingDetail(CatalogoResultDTO SettingDetail)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _configuracionesService.SaveCatSysConfiguracion(SettingDetail);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SaveMessageDetail")]
        public async Task<HttpResponseMessage> SaveMessageDetail(CatalogoResultDTO SettingDetail)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _configuracionesService.SaveCatSysLenguajeCiudadano(SettingDetail);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(PagedInformationDTO<CatalogoResultDTO>))]
        [HttpPost]
        [Route("Messages")]
        public async Task<HttpResponseMessage> GetMessages(BasePageInformationDTO Paginado)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _configuracionesService.ConsultaSysLenguajeCiudadano(Paginado, true);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [HttpGet]
        [Route("unlockUser/{requestId}")]
        public async Task<HttpResponseMessage> UnlockUserByRequestId(int requestId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await
                        _requestDomainService.DesbloquearUsuarioPorSolicitudId(requestId);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Obtiene la lista de estatus
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<EstadoCitaDTO>))]
        [HttpGet]
        [Route("Status")]
        public async Task<HttpResponseMessage> GetStatusCita()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _requestDomainService.ObtenerStatusCita();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpGet]
        [Route("SaveStatus")]
        public async Task<HttpResponseMessage> SaveStatusAppointment(int requestId, int statusId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var personInfo = await _requestDomainService.GetRequestByIdAsync(requestId);

                var result = await _requestDomainService.ActulizarEstadoCita(requestId, statusId);


                if (statusId == EnumTipoEdoCita.NoAsistio)
                    await SendMail(EnumConsCorreo.CuerpoCorreoCitaInasistencia,
                        EnumConsCorreo.TituloCorreoCitaInasistencia, personInfo);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        private async Task<bool> SendMail(string Mensaje, string TituloCorreo, RequestResultDTO PersonInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(PersonInfo.Promovente.CorreoElectronico)) return false;

                Func<string, string> EvaluarParametro = delegate (string Parametro) {
                    return string.IsNullOrEmpty(Parametro) ? string.Empty : Parametro;
                };

                var lista = new List<string> {Mensaje, TituloCorreo,EnumConsCorreo.NotaBloqueoCitas};

                var configuracion = await new LenguajeCiudadanoDAO().LenguajeCiudadanoServerAsync(lista);

                bool esUsuarioBloqueado = new CitaDAO().BloqueoPersona(new PersonaDTO { CURP= PersonInfo.Paciente.CURP});

                string tituloCorreo;
                string mensajeCorreo;
               
                configuracion.TryGetValue(Mensaje, out mensajeCorreo);
                configuracion.TryGetValue(TituloCorreo, out tituloCorreo);

                String CuerpoHTMLCorreo = ConfiguracionHelper.LeerTextoRuta(ConfiguracionHelper.ObtenerConfiguracion(Enumeracion.EnumSysConfiguracion.CuerpoDelCorreo));

                if (CuerpoHTMLCorreo != null)
                {
                    Mensaje = CuerpoHTMLCorreo.Replace("\n", string.Empty).Replace("[MensajeCorreo]", EvaluarParametro(mensajeCorreo));

                    Mensaje = Mensaje.Replace("[NumeroFolio]", EvaluarParametro(PersonInfo.Folio))
                    .Replace("[NombreCompleto]", (EvaluarParametro(PersonInfo.Promovente.Nombre) + " " + EvaluarParametro(PersonInfo.Promovente.Paterno) + " " + EvaluarParametro(PersonInfo.Promovente.Materno)))
                    .Replace("[FechaCita]", EvaluarParametro(PersonInfo.Cita.FechaCita.ToShortDateString()))
                    .Replace("[HoraCita]", EvaluarParametro(PersonInfo.Cita.HoraCita.ToString()));

                    if (esUsuarioBloqueado)
                    {
                        string notaBloqueoPaciente;
                        configuracion.TryGetValue(EnumConsCorreo.NotaBloqueoCitas, out notaBloqueoPaciente);

                        Mensaje = Mensaje.Replace("[Notas]", notaBloqueoPaciente);
                    }
                    else { Mensaje = Mensaje.Replace("[Notas]", string.Empty); }

                }

                var promoventeService = new PromoventeService();
                var conceptoTipoTramite = PersonInfo.CatTipoTramiteConcepto;

                if (tituloCorreo != null && (tituloCorreo.IndexOf("[NombreTramite]") != -1 && tituloCorreo.IndexOf("[NombreTramite]") != 0)) TituloCorreo = tituloCorreo.Replace("{NombreTramite}", conceptoTipoTramite);
                else TituloCorreo = string.Concat(tituloCorreo.Replace(".", "").TrimEnd(), " ", conceptoTipoTramite.Replace(".", "").TrimEnd(),".");

                await _mailService.SendMailAsync(PersonInfo.Promovente.CorreoElectronico, TituloCorreo, Mensaje);

                return true;
            }
            catch (Exception ex)
            {
                //LoggerException(ex);
                return false;
            }
        }



        /// <summary>
        ///     Regresa las listas de configuracion para el calendario
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(CalendarListDTO))]
        [HttpGet]
        [Route("AdmninistratorSchedule")]
        public async Task<HttpResponseMessage> GetSheduleList(int? RequestTypeId, int delegationId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = new CalendarListDTO
                {
                    Schedules = await _calendarDomainService.ObtenerHorariosAsync(RequestTypeId, delegationId),
                    NonLaborableDays =
                        await _calendarDomainService.ObtenerDiasNoLaborablesAsync(RequestTypeId, delegationId),
                    SpecialSchedules =
                        await _calendarDomainService.ObtenerDiasEspecialesAsync(RequestTypeId, delegationId)
                };
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Guarda la lista de configuraciones de citas
        /// </summary>
        /// <param name="saves"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SaveSchedules")]
        public async Task<HttpResponseMessage> SaveSchedules(List<Schedule> saves)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GuardarHorariosAsync(saves);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Guarda el horario de un dia especial
        /// </summary>
        /// <returns>Número de registros insertados</returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SaveSpecialScheduleDays")]
        public async Task<HttpResponseMessage> SaveDiaEspecialDTOSchedule(List<HorarioDiaEspecialDTO> specialSchedulesn)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GuardarHorariosDiasEspecialesAsync(specialSchedulesn);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Guarda la lista de dias no laborales
        /// </summary>
        /// <param name="specialDays"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SaveNonLaborableDays")]
        public async Task<HttpResponseMessage> SaveNonLaborableDays(List<HorarioDiaEspecialDTO> specialDays)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.GuardaDiasNoLaboralesOEspecialesAsync(specialDays);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Borra el dia no laborable
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("DeleteNonLaboraleDays")]
        public async Task<HttpResponseMessage> DeleteNonLaboraleDays(HorarioDiaEspecialDTO specialDay)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.DeleteNonLaboraleOEspecialDaysAsync(specialDay);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Borra los horarios asignados al día especial
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <param name="UnidadAtencionId"></param>
        /// <param name="HorarioInicio"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpGet]
        [Route("DeleteSpecialScheduleDays")]
        public async Task<HttpResponseMessage> DeleteSpecialScheduleDays(int? SolicitudId, int UnidadAtencionId,
            TimeSpan HorarioInicio)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result =
                    await _calendarDomainService.EliminarHorarios(SolicitudId, UnidadAtencionId, HorarioInicio, null,
                        true);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Elimina los horarios
        /// </summary>
        /// <param name="UnidadAtencionId"></param>
        /// <param name="HorarioInicio"></param>
        /// <param name="CatTipoDiaSemanaId"></param>
        /// <param name="SolicitudId"></param>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpGet]
        [Route("DeleteSchedule")]
        public async Task<HttpResponseMessage> DeleteSchedule(int UnidadAtencionId, TimeSpan HorarioInicio,
            int? CatTipoDiaSemanaId, int? SolicitudId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await _calendarDomainService.EliminarHorarios(SolicitudId, UnidadAtencionId, HorarioInicio,
                    CatTipoDiaSemanaId, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [HttpPost]
        [Route("Catalogo")]
        public async Task<HttpResponseMessage> ObtenerCatalogo(CatalogoRequestDTO CatalogoRequest)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var catalogo = await new CatalogosService().ConsultaCatalogoPorIdCatalogo(CatalogoRequest);

                return Request.CreateResponse(HttpStatusCode.OK, catalogo);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [HttpPost]
        [Route("ActulizaCatalogo")]
        public async Task<HttpResponseMessage> ActualizarDetalleCatalogo(CatalogoDTO CatalogoRequest)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var catalogo = await new CatalogosService().ActualizarCatalogo(CatalogoRequest);

                return Request.CreateResponse(HttpStatusCode.OK, catalogo);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(AsignacionesTramiteUnidadDTO))]
        [HttpGet]
        [Route("AsignacionesTramiteUnidad/{id}")]
        public async Task<HttpResponseMessage> GetAsignacionesTramiteUnidad(int id)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var catalogo = await new CatalogosService().ConsultaAsignacionesTramiteUnidad(id, null);

                return Request.CreateResponse(HttpStatusCode.OK, catalogo);
            });
        }


        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [Route("SetAsignacionesTramiteUnidad")]
        public async Task<HttpResponseMessage> GetAsignacionesTramiteUnidad(
            AsignacionesTramiteUnidadDTO AsignacionesTramiteUnidad)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await new CatalogosService().AsignacionesTramiteUnidad(AsignacionesTramiteUnidad);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<CatalogoDTO>))]
        [HttpGet]
        [Route("Catalogos")]
        public async Task<HttpResponseMessage> Catalogos()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var catalogos = await new CatalogosService().ObtenerCatalogos();

                return Request.CreateResponse(HttpStatusCode.OK, catalogos);
            });
        }
    }
}