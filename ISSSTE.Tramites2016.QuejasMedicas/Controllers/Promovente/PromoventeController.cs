using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Helpers;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.Common.Mail;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Promovente
{
    public class PromoventeController : BaseController
    {
        private readonly IMailService _mailService;

        public PromoventeController(IMailService MailService)
        {
            _mailService = MailService;
        }

        /// <summary>
        ///     Página principal del Promovente
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public ActionResult Index(int CatTipoTramiteId = 0)
        {
            var promoventeService = new PromoventeService();
            var existeTramite = new PromoventeService().SeEncuentraTramite(CatTipoTramiteId);

            if (!existeTramite)
                return View("Error");

            Session.RemoveAll();
            var lenguajeCiudadanoService = new LenguajeCiudadanoService();
            Enumeracion.MensajesServidor = lenguajeCiudadanoService.ConsultaLenguajeCiudadanoServer();

            var listaMensajesVista = lenguajeCiudadanoService.ConsultaLenguajeCiudadano();

            TempData["Sistema"] = promoventeService.ConceptoTipoTramite(CatTipoTramiteId);
            IdSesionTipoTramite = CatTipoTramiteId;

            return View(listaMensajesVista);
        }


        public ActionResult Informacion(int CatTipoTramiteId)
        {
            var promoventeService = new PromoventeService();

            var existeTramite = new PromoventeService().SeEncuentraTramite(CatTipoTramiteId);

            if (!existeTramite)
                return View("Error");

            var promoventeDto = promoventeService.ConsultaListado(CatTipoTramiteId, true);

            promoventeDto.DuplaValores = new DuplaValoresDTO
            {
                Valor = "Información general sobre este trámite"
                //Informacion = "Información general del trámite y los requisitos que se deben cumplir."
            };

            return PartialView(promoventeDto);
        }

        public ActionResult InformacionAdicional()
        {
            var promoventeService = new PromoventeService();
            var promoventeDto = promoventeService.ConsultaListado(IdSesionTipoTramite, false);

            promoventeDto.DuplaValores = new DuplaValoresDTO
            {
                Valor = "Más información sobre este trámite",
                Informacion = "Información adicional a este trámite."
            };

            return PartialView(promoventeDto);
        }


        /// <summary>
        ///     Calendario para agendar citas
        /// </summary>
        /// <param name="CalendarioDto"></param>
        /// <returns></returns>
        public ActionResult Calendario(CalendarioDTO CalendarioDto)
        {
            var catalogoService = new CatalogoService();
            CalendarioDto.CatTipoTramiteId = IdSesionTipoTramite;
            CalendarioDto.IdFechaLimite = IdFechaLimite;
            TempData["Fecha"] = CalendarioDto.MesAnio;
            var respuesta = catalogoService.GetBusquedaHorario(CalendarioDto);

            return PartialView(respuesta);
        }

        /// <summary>
        ///     Consulta de una cita
        /// </summary>
        /// <param name="CitaDto"></param>
        /// <returns></returns>
        public ActionResult VerSolicitud(CitaDTO CitaDto)
        {
            Enumeracion.MensajesServidor = new LenguajeCiudadanoService().ConsultaLenguajeCiudadanoServer();

            if (string.IsNullOrEmpty(CitaDto.RespuestaCaptcha))
                return Json(
                Enumeracion.EnumVarios.Prefijo +
                Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.ErrorCaptchaNoProporcionada],
                JsonRequestBehavior.AllowGet);

            bool esCaptchaValido = CaptchaService.ValidarCaptcha(CitaDto.RespuestaCaptcha);

            if (!esCaptchaValido)
                return Json(
                Enumeracion.EnumVarios.Prefijo +
                Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.ErrorCaptchaNoValido],
                JsonRequestBehavior.AllowGet);

            var citaService = new CitaService();
            CitaDto = citaService.GetCita(CitaDto);

            if (CitaDto == null)
            {
                return Json(
                    Enumeracion.EnumVarios.Prefijo +
                    Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaNoEncontrada],
                    JsonRequestBehavior.AllowGet);
            }


            CitaDto.EsRepresentante = (CitaDto.Promovente.CURP != CitaDto.Paciente.CURP);
            CitaDto.ConFolio = true;
            return PartialView(CitaDto);
        }

        public ActionResult VerSolicitudCompleta(CitaDTO CitaDto)
        {
            var citaService = new CitaService();
            CitaDto = citaService.GetCita(CitaDto);

            if (CitaDto == null)
            {
                return Json(
                    Enumeracion.EnumVarios.Prefijo +
                    Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaNoEncontrada],
                    JsonRequestBehavior.AllowGet);
            }


            CitaDto.EsRepresentante = (CitaDto.Promovente.CURP != CitaDto.Paciente.CURP);

            return PartialView(CitaDto);
        }




        public ActionResult Agenda()
        {
            return PartialView();
        }

        public ActionResult Horario()
        {
            return PartialView();
        }

        public ActionResult Confirmacion()
        {
            return PartialView();
        }

        public ActionResult Consulta()
        {
            var valor = new DuplaValoresDTO
            {
                Valor = "Consulta de tu cita"                
            };
            return PartialView(valor);
        }

        public ActionResult Cancelacion()
        {
            var valor = new DuplaValoresDTO
            {
                Valor = "Cancelación de tu cita",
                Id = Enumeracion.EnumVarios.ConEliminacion                
            };
            return PartialView("Consulta", valor);
        }

        /// <summary>
        ///     Solicitud con datos de vista
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerSolicitudVista(GeneralCitaDTO GeneralCitaDto)
        {
            //var generalCitaDto = Newtonsoft.Json.JsonConvert.DeserializeObject<GeneralCitaDTO>(Cadena);
            var citaDto = new CitaDTO();

            DebugLog("Entró a PromoventeController.VerSolicitudVista");
            try
            {
                citaDto = new CitaDTO
                {
                    Fecha = Convert.ToDateTime(GeneralCitaDto.FechaFinal),
                    HoraInicio = GeneralCitaDto.HorarioFinal,
                    Unidad_Atencion = GeneralCitaDto.Unidad_Atencion,
                    UnidadMedicaId = GeneralCitaDto.UnidadMedicaId,
                    Unidad_Medica = GeneralCitaDto.Unidad_Medica,
                    Promovente = GetPromoventeVista(GeneralCitaDto),
                    Paciente = GeneralCitaDto.EsRepresentante ? GetPacienteVista(GeneralCitaDto) : new PersonaDTO(),
                    EsRepresentante = GeneralCitaDto.EsRepresentante,
                    TramiteUnidadAtencionId= GeneralCitaDto.TramiteUnidadAtencionId,
                    DomicilioId = GeneralCitaDto.DomicilioId,
                    ConFolio = false
                };


                var citaService = new CitaService();
                citaService.GetDomicilio(0, citaDto);
            }
            catch (Exception ex)
            {
                EscribirLog(new ExceptionContext(ControllerContext, ex));
            }

            return PartialView("../Promovente/VerSolicitud", citaDto);
        }

        /// <summary>
        ///     Cancelación de una cita
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        public async Task<ActionResult> CancelarCita(GeneralCitaDTO GeneralCitaDto)
        {
            var citaService = new CitaService();

            var citaDto = new CitaDTO {SolicitudId = GeneralCitaDto.SolicitudId};
            citaDto = citaService.GetCita(citaDto);

            if(!EsSolicitudCancelable(citaDto))
                return Json(
                    Enumeracion.EnumVarios.Prefijo +
                    Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.ErrorExpiracionTiempoCancelacion],
                    JsonRequestBehavior.AllowGet);

            var mensaje = citaService.CancelarCita(GeneralCitaDto);

            if (!string.IsNullOrEmpty(mensaje) && !mensaje.Contains(Enumeracion.EnumVarios.Prefijo))
            {

                await SendMail(Enumeracion.EnumConsCorreo.CuerpoCorreoCitaCancelada,
                    Enumeracion.EnumConsCorreo.TituloCorreoCitaCancelada,Enumeracion.EnumConsCorreo.NotaCitaCancelada, citaDto);
            } 

            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Agendar una cita por horario
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        public async Task<ActionResult> AgendarCita(GeneralCitaDTO GeneralCitaDto)
        {
            var numeroFolio = Enumeracion.EnumVarios.Prefijo + "Excepción general";

            try
            {
                if (string.IsNullOrEmpty(GeneralCitaDto.RespuestaCaptcha))
                    return Json(
                    Enumeracion.EnumVarios.Prefijo +
                    Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.ErrorCaptchaNoProporcionada],
                    JsonRequestBehavior.AllowGet);

                bool esCaptchaValido = CaptchaService.ValidarCaptcha(GeneralCitaDto.RespuestaCaptcha);

                if (!esCaptchaValido)
                    return Json(
                    Enumeracion.EnumVarios.Prefijo +
                    Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.ErrorCaptchaNoValido],
                    JsonRequestBehavior.AllowGet);

                var citaDto = new CitaDTO();
                var citaService = new CitaService();
                numeroFolio = citaService.AgendarCita(GeneralCitaDto);


                if (!string.IsNullOrEmpty(numeroFolio) && !numeroFolio.Contains(Enumeracion.EnumVarios.Prefijo))
                {
                    GeneralCitaDto.NumeroFolio = numeroFolio;

                    citaDto.NumeroFolio = numeroFolio;
                    citaDto = citaService.GetCita(GeneralCitaDto);

                    await SendMail(Enumeracion.EnumConsCorreo.CuerpoCorreoCitaAgendada,
                        Enumeracion.EnumConsCorreo.TituloCorreoCitaAgendada,Enumeracion.EnumConsCorreo.NotaCitaAgendada, citaDto);
                }
            }
            catch (Exception ex)
            {
                //Log
                EscribirLog(new ExceptionContext(ControllerContext, ex));
            }

            return Json(numeroFolio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerHomoclaveTramite(int CatTipoTramiteId)
        {
            var catalogoService = new CatalogoService();

            var homoclave = catalogoService.GetHomClave(CatTipoTramiteId);

            return Json(homoclave, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Bloqueo de una persona por número de citas
        /// </summary>
        /// <param name="PersonaDto"></param>
        /// <returns></returns>
        public JsonResult BloqueoPersona(PersonaDTO PersonaDto)
        {
            var citaService = new CitaService();
            var resultado = citaService.BloqueoPersona(PersonaDto);
            var mensaje = "";
            if (resultado)
                mensaje = Enumeracion.EnumVarios.Prefijo +
                          Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.UsuarioBloquedo];
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CitasVigentesPorTramite(PersonaDTO PersonaDto)
        {
            var citaService = new CitaService();
            PersonaDto.CatTipoTramiteId = IdSesionTipoTramite;
            var conteo = citaService.CitasVigentesPorTramite(PersonaDto);
            var mensaje = "";
            if (conteo >= 1)
                mensaje = Enumeracion.EnumVarios.Prefijo +
                          Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaPendiente];
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }


        #region Métodos privados

        /// <summary>
        ///     Datos del paciente en vista
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        private PersonaDTO GetPacienteVista(GeneralCitaDTO GeneralCitaDto)
        {
            var persona2Dto = new PersonaDTO
            {
                PersonaId = -1,
                Nombre = GeneralCitaDto.NombrePaciente,
                Paterno = GeneralCitaDto.Apellido1Paciente,
                Materno = GeneralCitaDto.Apellido2Paciente,
                CURP = GeneralCitaDto.CURPPaciente,
                Lada = GeneralCitaDto.LadaPaciente,
                TelFijo = GeneralCitaDto.TelefonoPaciente,
                TelMovil = GeneralCitaDto.CelularPaciente,
                CorreoElectronico = GeneralCitaDto.CorreoPaciente
            };
            persona2Dto.EsMasculino = Utilerias.GetSexo(persona2Dto.CURP);
            persona2Dto.FechaNacimiento = Utilerias.GetFechaNacimiento(persona2Dto.CURP);
            return persona2Dto;
        }

        /// <summary>
        ///     Datos del promovente en vista
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        private PersonaDTO GetPromoventeVista(GeneralCitaDTO GeneralCitaDto)
        {
            var persona1Dto = new PersonaDTO
            {
                Nombre = GeneralCitaDto.NombrePromovente,
                Paterno = GeneralCitaDto.Apellido1Promovente,
                Materno = GeneralCitaDto.Apellido2Promovente,
                CURP = GeneralCitaDto.CURPPromovente,
                Lada = GeneralCitaDto.LadaPromovente,
                TelFijo = GeneralCitaDto.TelefonoPromovente,
                TelMovil = GeneralCitaDto.CelularPromovente,
                CorreoElectronico = GeneralCitaDto.CorreoPromovente
            };
            persona1Dto.EsMasculino = Utilerias.GetSexo(persona1Dto.CURP);
            persona1Dto.FechaNacimiento = Utilerias.GetFechaNacimiento(persona1Dto.CURP);
            return persona1Dto;
        }


        private bool EsSolicitudCancelable(CitaDTO CitaDTO)
        {
            DateTime FechaHoraActual = DateTime.Now;

            TimeSpan DiferenciaFechas = Convert.ToDateTime(CitaDTO.FechaConHora)-FechaHoraActual;

            if (DiferenciaFechas.TotalHours <= 24)
                return false;
            else return true;

        }

        private async Task<bool> SendMail(string Mensaje, string TituloCorreo,String NotaCorreo, CitaDTO CitaDto)
        {
            try
            {
                Func<string, string> EvaluarParametro = delegate (string Parametro){
                    return string.IsNullOrEmpty(Parametro) ? string.Empty : Parametro; };

                if (string.IsNullOrEmpty(CitaDto.Promovente.CorreoElectronico)) return false;

                var lista = new List<string> { Mensaje, TituloCorreo, NotaCorreo };

                var configuracion = await new LenguajeCiudadanoDAO().LenguajeCiudadanoServerAsync(lista);

                string tituloCorreo;
                string mensajeCorreo;
                string notaCorreo;

                configuracion.TryGetValue(Mensaje, out mensajeCorreo);
                configuracion.TryGetValue(TituloCorreo, out tituloCorreo);
                configuracion.TryGetValue(NotaCorreo, out notaCorreo);

                String CuerpoHTMLCorreo = ConfiguracionHelper.LeerTextoRuta(ConfiguracionHelper.ObtenerConfiguracion(Enumeracion.EnumSysConfiguracion.CuerpoDelCorreo));

                if (CuerpoHTMLCorreo != null)
                {
                    Mensaje = CuerpoHTMLCorreo.Replace("\n",string.Empty).Replace("[MensajeCorreo]", EvaluarParametro(mensajeCorreo));

                    Mensaje = Mensaje.Replace("[NumeroFolio]", EvaluarParametro(CitaDto.NumeroFolio))
                    .Replace("[NombreCompleto]", (EvaluarParametro(CitaDto.Promovente.Nombre) + " " + EvaluarParametro(CitaDto.Promovente.Paterno) + " " + EvaluarParametro(CitaDto.Promovente.Materno)))
                    .Replace("[FechaCita]", EvaluarParametro(CitaDto.FechaCitaVista))
                    .Replace("[HoraCita]", EvaluarParametro(CitaDto.HoraCitaVista));
                   
                    Mensaje = Mensaje.Replace("[Notas]", EvaluarParametro(notaCorreo));
                }

                var promoventeService = new PromoventeService();
                var conceptoTipoTramite = promoventeService.ConceptoTipoTramite(CitaDto.CatTipoTramiteId);

                if (tituloCorreo != null && (tituloCorreo.IndexOf("[NombreTramite]")!=-1&& tituloCorreo.IndexOf("[NombreTramite]") != 0)) TituloCorreo = tituloCorreo.Replace("{NombreTramite}", conceptoTipoTramite);
                else TituloCorreo = string.Concat(tituloCorreo.Replace(".","").TrimEnd()," ", conceptoTipoTramite.Replace(".",""),".");

                await _mailService.SendMailAsync(CitaDto.Promovente.CorreoElectronico, TituloCorreo, Mensaje);

                return true;
            }
            catch (Exception ex)
            {
                EscribirLog(new ExceptionContext(ControllerContext, ex));
                return false;
            }
        }



		#endregion


		public ActionResult PrivacidadIntegral(string CatTipoTramiteId)
		{
			//ViewBag.TipoTramite = CatTipoTramiteId;
			ViewData["TipoTramite"] = CatTipoTramiteId;
			return View();
		}
	}
}