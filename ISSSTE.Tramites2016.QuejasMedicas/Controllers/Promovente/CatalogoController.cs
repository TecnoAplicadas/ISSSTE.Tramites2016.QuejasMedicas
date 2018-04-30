using System.Collections.Generic;
using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService;
using ISSSTE.Tramites2016.QuejasMedicas.Helpers;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using System.Web;
using System.Linq;
using System;
using ISSSTE.Tramites2016.Common.Security.Core;
using ISSSTE.Tramites2016.Common.Security.Helpers;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Promovente
{
    public class CatalogoController : BaseController
    {

        public ActionResult GetUADyCS(bool SinTodos = true)
        {
            var catalogoService = new CatalogoService();
            var combo = catalogoService.GetUadyCs(IdSesionTipoTramite);
            if (IdSesionTipoTramite == 0)
                SinTodos = false;
            var respuesta = Tag.GeneracionCuerpoSelect(SinTodos, combo);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserUADyCS(bool SinTodos = false)
        {
            var delegationIds = GelDelegationIds();

            var catalogoService = new CatalogoService();
            var combo = catalogoService.GetUserUADyCS(delegationIds);

            var respuesta = Tag.GeneracionCuerpoSelect(false, combo);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTramiteUnidadMedicaEspecial(CalendarioDTO CalendarioDto)
        {
            var catalogoService = new CatalogoService();
            var combo = catalogoService.GetTramiteUnidadMedicaEspecial(CalendarioDto);
            var respuesta = Tag.GeneracionCuerpoSelect(true, combo);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusquedaHorarioUADyCS(int TramiteUnidadAtencionId)
        {
            var catalogoService = new CatalogoService();
            IdFechaLimite = catalogoService.GetBusquedaHorarioUadyCs(TramiteUnidadAtencionId);
            return Json(IdFechaLimite, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     combo de unidades por tipo de trámite
        /// </summary>
        /// <param name="CalendarioDto"></param>
        /// <returns></returns>
        public JsonResult GetTramiteUnidadMedica(CalendarioDTO CalendarioDto)
       {
            var catalogoService = new CatalogoService();
            //CalendarioDto.CatTipoTramiteId = IdSesionTipoTramite;
            var combo = catalogoService.GetTramiteUnidadMedica(CalendarioDto);
            var respuesta = Tag.GeneracionCuerpoSelect(false, combo);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     búsqueda predictiva de unidades
        /// </summary>
        /// <param name="Concepto"></param>
        /// <returns></returns>
        public JsonResult GetBusqueda(string Concepto)
        {
            var catalogoService = new CatalogoService();
            var respuesta = catalogoService.GetBusqueda(Concepto, IdSesionTipoTramite);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     búsqueda de horarios por día
        /// </summary>
        /// <param name="Concepto"></param>
        /// <param name="TramiteUnidadAtencionId"></param>
        /// <returns></returns>
        public JsonResult GetBusquedaDia(string Concepto, int TramiteUnidadAtencionId)
        {
            var catalogoService = new CatalogoService();
            var respuesta = catalogoService.GetBusquedaDia(Concepto, TramiteUnidadAtencionId);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusquedaTipoTramite(bool ConTodos=false)
        {
            var catalogoService = new CatalogoService();
            var respuesta = catalogoService.GetBusquedaTipoTramite();
            var cadena = SelectHtml(respuesta, ConTodos);
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusquedaEstadoCita(bool ConTodos=false)
        {
            var catalogoService = new CatalogoService();
            var respuesta = catalogoService.GetBusquedaEstadoCita();
            var cadena = SelectHtml(respuesta, ConTodos);
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }

        #region Métodos privados

        private string SelectHtml(IEnumerable<DuplaValoresDTO> Elementos, bool ConTodos=false)
        {
            var cadena = "";
            if (ConTodos)
            {
                cadena += "<option value='0'>Selecciona</option>";
            }
            foreach (var item in Elementos)
                cadena += "<option value='" + item.Id + "'>" + item.Valor + "</option>";
            return cadena;
        }


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

        #endregion
    }
}