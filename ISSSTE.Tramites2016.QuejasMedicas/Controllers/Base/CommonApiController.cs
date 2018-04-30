using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.ComunDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CuentaDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Helpers;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Common.Web.Http;
using ISSSTE.Tramites2016.Common.Security.Core;
using ISSSTE.Tramites2016.Common.Security.Helpers;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    [AuthorizeByConfig("AllAdminRoles")]
    [RoutePrefix("api/Common")]
    public class CommonApiController : BaseApiController
    {
        /// <summary>
        ///     Constructor del controlador
        /// </summary>
        /// <param name="commonDomainService"></param>
        public CommonApiController(ICommonDomainService commonDomainService)
        {
            if (commonDomainService == null) throw new ArgumentNullException(nameof(commonDomainService));
            CommonDomainService = commonDomainService;
        }

        public ICommonDomainService CommonDomainService { get; set; }


        /// <summary>
        ///  Menu del usuario logeado
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(MenuDTO))]
        [HttpGet]
        [Route("User/Menu")]
        public HttpResponseMessage GetUserMenu()
        {
            return HandleOperationExecution(() =>
            {
                var owinContext = Request.GetOwinContext();
                var rolesNames = new List<string>();

                var roles = owinContext.GetAuthenticatedUserRoles();

                if (roles != null)
                    rolesNames = roles.Select(R => R.Name).ToList();

                var objetoSerializable = new UsuarioService().ConstruyeMenuUsuario(rolesNames);

                return Request.CreateResponse(HttpStatusCode.OK, objetoSerializable);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(NotificacionDTO))]
        [HttpGet]
        [Route("Notifications")]
        public async Task<HttpResponseMessage> GetNotificationsByUADyCS()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var delegationIds = GelDelegationIds();

                var dates = FechaConversion.GetDates(DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToShortDateString());

                var result = await CommonDomainService.ObtenerNotificacionesPorUADyCSAsync(dates, delegationIds);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Menu del usuario logeado
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(DelegacionDTO))]
        [HttpGet]
        [Route("User/DelegationsByConfig/{RequestTypeId}")]
        public async Task<HttpResponseMessage> GetDelegationsByConfig(int RequestTypeId)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var delegationIds = GelDelegationIds();

                var result = await CommonDomainService.GetDelegationsByConfig(RequestTypeId, delegationIds);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene el catalogo de entidades
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<CatTipoEntidadDTO>))]
        [HttpGet]
        [Route("Entities")]
        public async Task<HttpResponseMessage> GetEntities()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await CommonDomainService.ObtenerEntidadesFederativasAsync();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene las delegaciones  a las que pertenece el usuario en sesion.
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(DelegacionDTO))]
        [HttpGet]
        [Route("User/Delegations")]
        public async Task<HttpResponseMessage> GetUserDelegations()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var delegationIds = GelDelegationIds();

                var result = await CommonDomainService.ObtenerUADyCSPorUADyCSIdsAsync(delegationIds);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///  Obtiene la lista de delegaciones activas
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(DelegacionDTO))]
        [HttpGet]
        [Route("Delegations")]
        public async Task<HttpResponseMessage> Getelegations()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await CommonDomainService.ObtenerUADyCSAsync();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        /// Obtiene la lista de estatus activos
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<EstadoCitaDTO>))]
        [HttpGet]
        [Route("NextStatus")]
        public async Task<HttpResponseMessage> GetStatus()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await CommonDomainService.ObtenerEStatusSiguientesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene el catalogo de tramites activos
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<TramiteDTO>))]
        [HttpGet]
        [Route("RequestTypes")]
        public async Task<HttpResponseMessage> GetRequestTypes()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await CommonDomainService.ObtenerTiposDeTramiteAsync();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }


        /// <summary>
        ///     Obtiene el catalogo de dias de la semana activos
        /// </summary>
        /// <returns></returns>
        [AuthorizeByConfig("AllAdminRoles")]
        [ResponseType(typeof(List<DiaSemanaDTO>))]
        [HttpGet]
        [Route("Weekdays")]
        public async Task<HttpResponseMessage> GetWeekdays()
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                var result = await CommonDomainService.ObtenerDiasSemanaAsync();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            });
        }

        /// <summary>
        ///     Obtiene los identificadores de las delegaciones del issste del usuario en sesion
        /// </summary>
        /// <returns>Un arreglo de identificadores de delegaciones</returns>
        private int[] GelDelegationIds()
        {
            var owinContext = Request.GetOwinContext();

            var user = owinContext.GetAuthenticatedUser();

            if (user == null) return new int[0];

            var delegations = user.GetPropertyValues(IsssteUserPropertyTypes.UADyCS);

            if (delegations == null) return new int[0];

            var delegationIds = (from d in delegations
                select Convert.ToInt32(d.Value)).ToArray();

            return delegationIds;
        }
    }
}