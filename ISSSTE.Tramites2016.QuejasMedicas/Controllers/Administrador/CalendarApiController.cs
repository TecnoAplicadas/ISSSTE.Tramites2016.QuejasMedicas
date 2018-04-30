using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Helpers;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Common.Web.Http;
using ISSSTE.Tramites2016.Common.Security.Helpers;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    [AuthorizeByConfig("AllAdminRoles")]
    [RoutePrefix("api/Calendar")]
    public class CalendarApiController : BaseApiController
    {
        /// <summary>
        ///     Dominio del calendario
        /// </summary>
        private readonly ICalendarDomainService _calendarDomainService;


        public CalendarApiController(ICalendarDomainService CalendarDomainService)
        {
            _calendarDomainService = CalendarDomainService;
        }

        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(CalendarDTO))]
        [HttpGet]
        [Route("Appointments")]
        public async Task<HttpResponseMessage> GetAppointments(string dates, string query = null, int? statusId = null,
            int? delegationId = null, int? RequestTypeId = null)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var owinContext = Request.GetOwinContext();

                var user = owinContext.GetAuthenticatedUser();

                var delegations = new List<int>();

                if (delegationId != null)
                {
                    delegations.Add(Convert.ToUInt16(delegationId));
                    if (!user.GetUADyCS().Contains(Enumeracion.EnumVarios.TodasLasDelegaciones))
                        delegations = user.GetUADyCS().Intersect(delegations).ToList();
                }
                else
                {
                    delegations = user.GetUADyCS();
                }

                var fechas = FechaConversion.GetDates(dates);

                var result =
                    await
                        _calendarDomainService.GetAppointments(delegations, fechas, query, statusId, RequestTypeId);

                result.ListAppointments.ForEach(Cita =>
                {
                    Cita.Start = FechaConversion
                        .ConvertDateToTInMS(
                            Convert.ToDateTime(Cita.StartDate.ToString("dd/MM/yyyy") + " " + Cita.HoraCita))
                        .ToString();
                    Cita.End = FechaConversion
                        .ConvertDateToTInMS(Convert
                            .ToDateTime(Cita.StartDate.ToString("dd/MM/yyyy") + " " + Cita.HoraCita)
                            .AddMinutes(30))
                        .ToString();
                    Cita.Title = "Folio: " + Cita.NumeroFolio + ", Fecha/Hora de la cita:" +
                                 Cita.StartDate.ToString("dd/MM/yyyy") + " " + Cita.HoraCita;
                    Cita.Url = "RequestDetail?SolicitudId=" + Cita.SolicitudId.ToString();
                });

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }
    }
}