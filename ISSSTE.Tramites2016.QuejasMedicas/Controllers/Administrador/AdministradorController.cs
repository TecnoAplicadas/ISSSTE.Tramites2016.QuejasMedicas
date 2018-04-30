using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using Newtonsoft.Json;
using System.Linq;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.ReporteService;
using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.Controllers.Reporte;
using System.Text;
using System;
using System.Web;
using ISSSTE.Tramites2016.QuejasMedicas.Model;
using ISSSTE.Tramites2016.Common.Web.Mvc;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    [AuthorizeByConfig("AllAdminRoles")]
    [RoutePrefix("Administrador")]
    public class AdministradorController : BaseController
    {
        #region Solicitudes

        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult RequestDetail(int SolicitudId)
        {
            var requestDomain = new RequestDomainService();
            var model = requestDomain.GetRequestById(SolicitudId);
            return PartialView(model);
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult VerSolicitudCompleta(CitaDTO CitaDto)
        {
            var citaService = new CitaService();
            CitaDto = citaService.GetCita(CitaDto, true);

            if (CitaDto == null)
            {
                throw new QuejasMedicasException(Enumeracion.EnumVarios.Prefijo +
                    Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaNoEncontrada]);
            }

            CitaDto.EsRepresentante = (CitaDto.Promovente.CURP != CitaDto.Paciente.CURP);

            return PartialView(CitaDto);
        }

        #endregion Solicitudes

        #region Reportes

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult ReporteDinamico()
        {
            var lenguajeCiudadanoService = new LenguajeCiudadanoService();
            Enumeracion.MensajesServidor = lenguajeCiudadanoService.ConsultaLenguajeCiudadanoServer();

            return PartialView();
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult ReporteEstadistico()
        {
            return PartialView();
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult Filtros()
        {
            return PartialView();
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult FiltroGrafica(GeneralCitaDTO Filtros)
        {
            var reporteService = new ReporteService();
            var lista2 = reporteService.ListaReporte2(Filtros);
            return PartialView(lista2);
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public ActionResult CompendioCitas(GeneralCitaDTO Filtros)
        {
            if (Filtros.Encabezados == "null" || Filtros.Encabezados == "undefined")
                Filtros.Encabezados = null;
            if (Filtros.Llaves == "null" || Filtros.Llaves == "undefined")
                Filtros.Llaves = null;

            var encabezados = string.IsNullOrEmpty(Filtros.Encabezados)
                ? new List<string>()
                : Filtros.Encabezados.Split(',').ToList();
            var llaves = string.IsNullOrEmpty(Filtros.Llaves) ? new List<string>() : Filtros.Llaves.Split(',').ToList();

            var reporteService = new ReporteService();
            var listaResultados = reporteService.ListaReporte1(Filtros);
            var objetos = new List<IDictionary<string, object>>();
            foreach (var resultado in listaResultados)
            {
                var objeto = resultado.AsDictionary();
                objetos.Add(objeto);
            }

            Filtros.ListaRefleccion = objetos;
            Filtros.ListaEncabezados = encabezados;
            Filtros.ListaLlaves = llaves;
            Filtros.TotalRegistros = Enumeracion.TotalRegistros;

            return PartialView(Filtros);
        }

        [AuthorizeByConfig("AllAdminRoles")]
        public void jsCrearCompendioExcel()
        {
            var cadena = Request["JsonFiltros"];
            var filtros = JsonConvert.DeserializeObject<GeneralCitaDTO>(cadena);
            var encabezados = Request["Encabezados"].Split(',').ToList();
            var llaves = Request["Llaves"].Split(',').ToList();
            var caption = Request["Caption"];

            var reporteService = new ReporteService();
            filtros.ConPaginadorController = false;
            var listaResultados = reporteService.ListaReporte1(filtros);
            var header = HeaderConsultaExcel(encabezados, llaves);
            var body = BodyConsultaExcel(listaResultados, llaves);
            //var table = "<table><caption>" + UnEscape(caption) + "</caption>" + header + body + "</table>";
            var table = "<table>" + header + body + "</table>";
            RespuestaXls("Compendio_de_citas.xls", table);
        }

        [AuthorizeByConfig("AllAdminRoles")]
        private string HeaderConsultaExcel(IEnumerable<string> Encabezados, IReadOnlyList<string> Llaves)
        {
            var encabezado = new StringBuilder();
            var Estilo = " style='font-weight:bold;color:black;background-color: #FFC300' ";
            encabezado.Append("<thead><tr " + Estilo + ">");
            var i = 0;
            foreach (var header in Encabezados)
            {
                encabezado.Append("<td><div class='Encabezado " + Llaves[i] + "'>" + header + "</div></td>");
                i++;
            }
            encabezado.Append("</tr></thead>");
            return AcentosHTML(encabezado.ToString());
        }

        private string BodyConsultaExcel(IEnumerable<GeneralCitaDTO> Resultados, List<string> Llaves)
        {
            var renglones = new StringBuilder();
            renglones.Append("<tbody>");
            var estilo1 = " style='color:black;background-color:FONDO_COLOR' ";
            var estilo2 = estilo1.Replace("FONDO_COLOR", "white;");
            estilo1 = estilo1.Replace("FONDO_COLOR", " #e5ecf9;");

            var i = 0;
            foreach (var item in Resultados)
            {
                var resultadoRow = item.AsDictionary();
                var columnas = new StringBuilder();
                foreach (var label in Llaves)
                    try
                    {
                        var estilo = "";
                        if (label.Contains("EstadoTramite"))
                            estilo = " style='font-weight:bold;background-color:" + resultadoRow["ColorTramite"] + "' ";
                        else if (label.Contains("EstadoCita"))
                            estilo = " style='font-weight:bold;background-color:" + resultadoRow["ColorCita"] + "' ";
                        columnas.Append("<td " + estilo + ">" + resultadoRow[label] + "</td>");
                        //columnas.Append("<td>" + resultadoRow[label] + "</td>");
                    }
                    catch (Exception)
                    {
                        columnas.Append("<td>ERROR</td>");
                    }
                renglones.Append("<tr class='Row' " +
                                 (i % 2 == 0 ? estilo1 : estilo2)
                                 + ">" + columnas + "</tr>");
                i++;
            }
            renglones.Append("</tbody>");
            var auxiliar = renglones.ToString();
            auxiliar = auxiliar.Replace("True", "Si").Replace("False", "No");
            return AcentosHTML(auxiliar);
        }

        private string AcentosHTML(string Cadena)
        {
            Cadena = Cadena.Replace("Á", "&Aacute;");
            Cadena = Cadena.Replace("É", "&Eacute;");
            Cadena = Cadena.Replace("Í", "&Iacute;");
            Cadena = Cadena.Replace("Ó", "&Oacute;");
            Cadena = Cadena.Replace("Ú", "&Uacute;");
            Cadena = Cadena.Replace("á", "&aacute;");
            Cadena = Cadena.Replace("é", "&eacute;");
            Cadena = Cadena.Replace("í", "&iacute;");
            Cadena = Cadena.Replace("ó", "&oacute;");
            Cadena = Cadena.Replace("ú", "&uacute;");
            Cadena = Cadena.Replace("Ü", "&Uuml;");
            Cadena = Cadena.Replace("ü", "&uuml;");
            Cadena = Cadena.Replace("Ñ", "&Ntilde;");
            Cadena = Cadena.Replace("ñ", "&ntilde;");
            return Cadena;
        }

        private void RespuestaXls(string NombreArchivo, string Informacion)
        {
            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Charset = "";
            Response.ContentType = "application/xls";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AddHeader("Content-Type", "application/xls");
            Response.AddHeader("Content-Disposition", "inline;filename=" + NombreArchivo);
            Response.Write(Informacion);
            Response.Flush();
            Response.End();
        }


        #endregion Reportes
    
        #region Calendario
        public ActionResult month()
        {
            return PartialView();
        }

        public ActionResult month_day()
        {
            return PartialView();
        }

        public ActionResult events_list()
        {
            return PartialView();
        }

        public ActionResult year()
        {
            return PartialView();
        }

        public ActionResult year_month()
        {
            return PartialView();
        }

        public ActionResult week()
        {
            return PartialView();
        }

        public ActionResult day()
        {
            return PartialView();
        }


        public ActionResult week_days()
        {
            return PartialView();
        }

        #endregion Calendario
    }
}