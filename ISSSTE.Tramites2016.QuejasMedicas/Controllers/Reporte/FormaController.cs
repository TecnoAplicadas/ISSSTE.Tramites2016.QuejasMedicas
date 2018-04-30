using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using Newtonsoft.Json;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.ReporteService;
using System.Collections.Generic;
using ISSSTE.Tramites2016.Common.Web.Mvc;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Reporte
{


    public class FormaController : BaseController
    {
        public string ErrorEjecucion = string.Empty;

        // application/vnd.openxmlformats-officedocument.wordprocessingml.document  --> DOCX
        // application/msword                                                       --> DOC


        public async Task<FileResult> jsCrearDOCXTramite()
        {
            return await Task.Run(() =>
            {
                var configuracionService = new ConfiguracionService();
                var ruta = configuracionService.GetFormatoTramite(IdSesionTipoTramite);
                ruta = ruta.Replace("pdf", "docx");
                var bytesPdf = System.IO.File.ReadAllBytes(ruta);

                return File(bytesPdf, "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Requisitos.docx");
            });
        }


        public async Task<bool> jsCrearPDFTramite()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var configuracionService = new ConfiguracionService();
                    var ruta = configuracionService.GetFormatoTramite(IdSesionTipoTramite);
                    var bytesPdf = System.IO.File.ReadAllBytes(ruta);

                    Response.Clear();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Charset = "";
                    Response.BufferOutput = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=Requisitos.pdf");
                    Response.AddHeader("Content-Length", bytesPdf.Length.ToString());
                    Response.BinaryWrite(bytesPdf);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }
                return true;
            });
        }

        public async Task<bool> jsCrearImageJPG()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var contenedorOrigen =
                        new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaArchivos);
                    var idSesion = Session.SessionID;
                    var nombreArchivo = NombreSalida();
                    var guidNuevo = Guid.NewGuid();

                    var contenido = ProcesamientoGeneralImagen(idSesion, contenedorOrigen, guidNuevo, true, null);

                    EliminacionTemporales(contenedorOrigen, "Usuario_" + idSesion);
                    RespuestaJpg(nombreArchivo + ".jpg", contenido);
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return true;
            });
        }

        public async Task<bool> jsCrearImageExcelJPG()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var contenedorOrigen =
                        new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaArchivos);
                    var idSesion = Session.SessionID;
                    var nombreArchivo = NombreSalida();
                    var guidNuevo = Guid.NewGuid();

                    var contenido = ProcesamientoGeneralImagen(idSesion, contenedorOrigen, guidNuevo, true,null);
                    if (contenido != null)
                    {
                        var archivoDestino = Path.Combine(contenedorOrigen,
                            "Usuario_" + idSesion + "_" + guidNuevo + ".xls");
                        OpenImageToXLS(contenido, archivoDestino);
                        contenido = LecturaDOC(archivoDestino);
                        RespuestaXls(nombreArchivo + ".xls", contenido);
                    }
                    EliminacionTemporales(contenedorOrigen, "Usuario_" + idSesion);
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return true;
            });
        }

        public async Task<bool> jsCrearExcelReporteGrafico()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var cadena = Request["JsonFiltros"];
                    var filtros = JsonConvert.DeserializeObject<GeneralCitaDTO>(cadena);

                    var estadosCitas = new ReporteService().ListaReporte2(filtros);

                    var contenedorOrigen =
                        new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaArchivos);
                    var idSesion = Session.SessionID;
                    var nombreArchivo = NombreSalida();
                    var guidNuevo = Guid.NewGuid();

                    var contenido = ProcesamientoGeneralImagen(idSesion, contenedorOrigen, guidNuevo, true, null);
                    if (contenido != null)
                    {
                        var archivoDestino = Path.Combine(contenedorOrigen,
                            "Usuario_" + idSesion + "_" + guidNuevo + ".xls");
                        OpenImageToXLSReporteGrafico(estadosCitas,contenido, archivoDestino);
                        contenido = LecturaDOC(archivoDestino);
                        RespuestaXls(nombreArchivo + ".xls", contenido);
                    }
                    EliminacionTemporales(contenedorOrigen, "Usuario_" + idSesion);
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return true;
            });
        }

        public ActionResult Imagen_VP(DuplaValoresDTO DuplaValores)
        {
                try
                {
                    var contenedorOrigen =
                        new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaArchivos);
                    var idSesion = Session.SessionID;
                    var guidNuevo = Guid.NewGuid();

                    var esImagen = (DuplaValores.Id == 0);
                    var contenido = ProcesamientoGeneralImagen(idSesion, contenedorOrigen, guidNuevo, esImagen,DuplaValores.Informacion);
                    EliminacionTemporales(contenedorOrigen, "Usuario_" + idSesion);

                    string imageBase64Data = Convert.ToBase64String(contenido);

                    var encoding = (DuplaValores.Id == 0)
                        ? "data:image/jpeg;base64,{0}"
                        : "data:application/pdf;base64,{0}";
                    ViewBag.ImageData = string.Format(encoding, imageBase64Data);

                    return PartialView();
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return Json("ERROR",JsonRequestBehavior.AllowGet);
        }

        public async Task<bool> jsCrearPDF()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var contenedorOrigen =
                        new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaArchivos);
                    var idSesion = Session.SessionID;
                    var nombreArchivo = NombreSalida();
                    var guidNuevo = Guid.NewGuid();
                    var contenido = ProcesamientoGeneralImagen(idSesion, contenedorOrigen, guidNuevo, false, null);

                    if (contenido != null)
                        RespuestaPdf(nombreArchivo + ".pdf", contenido);
                    EliminacionTemporales(contenedorOrigen, "Usuario_" + idSesion);
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return true;
            });
        }

        public async Task<bool> jsCrearTXT()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var html = TratamientoContenidoPdf(false, null);
                    var nombreArchivo = NombreSalida();

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AddHeader("Content-Type", "text/plain");
                    Response.Charset = "";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreArchivo);
                    Response.Write(UnEscape(html));
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return true;
            });
        }

        public async Task<bool> jsCrearXLS()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var html = TratamientoContenidoPdf(true, null);
                    var nombreArchivo = NombreSalida();

                    Response.Clear();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Charset = "";
                    Response.ContentType = "application/xls";
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.AddHeader("Content-Type", "application/xls");
                    Response.AddHeader("Content-Disposition", "inline;filename=" + nombreArchivo);
                    Response.Write(UnEscape(html));
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    RespuestaError(ex);
                }

                return true;
            });
        }

        #region Métodos privados

        private void OpenImageToXLS(byte[] Contenido, string Excel)
        {
            using (var sl = new SLDocument())
            {
                var pic = new SLPicture(Contenido, ImagePartType.Png);
                pic.SetPosition(3, 2);
                sl.InsertPicture(pic);

                sl.SaveAs(Excel);
                sl.Dispose();
            }
        }


        private void OpenImageToXLSReporteGrafico(List<EstadoCitaCortoDTO> EstadosCitas,byte[] Contenido, string Excel)
        {
            using (var sl = new SLDocument())
            {
                var pic = new SLPicture(Contenido, ImagePartType.Png);
                pic.SetPosition(3, 2);
                sl.InsertPicture(pic);

                sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Grafica UADyCS");
                sl.AddWorksheet("Tabla Reporte");
                sl.SelectWorksheet("Tabla Reporte");

                int filaInicio = 6;
                int numeroColumnaInicio = 4;
                int numeroColumnaCuerpo = numeroColumnaInicio;
                int numeroFila = filaInicio;

                sl.SetCellValue(numeroFila-2, numeroColumnaInicio, "Reporte UADyCS");
                sl.SetCellValue(numeroFila, numeroColumnaInicio, "UADyCS");

                EstadosCitas.ForEach(fila =>
                {
                    numeroColumnaCuerpo = numeroColumnaInicio + 1;

                    if (numeroFila == filaInicio) {

                        fila.Valores.ForEach(estado => {//Descripcion del estado=> solicitado,atendido,..
                            sl.SetCellValue(numeroFila, numeroColumnaCuerpo, estado.Item1);numeroColumnaCuerpo++;
                        });
                    }

                    numeroFila++;

                    //Nombre de la UADyCS
                    sl.SetCellValue(numeroFila, numeroColumnaInicio, fila.Descripcion);

                    numeroColumnaCuerpo = numeroColumnaInicio;

                    fila.Valores.ForEach(estado => {

                        numeroColumnaCuerpo++;
 
                        //Cantidad del estado
                        sl.SetCellValue(numeroFila, numeroColumnaCuerpo, estado.Item2);
                    });

                });

                SLTable tbl = sl.CreateTable(filaInicio, numeroColumnaInicio, numeroFila, numeroColumnaCuerpo);
                tbl.SetTableStyle(SLTableStyleTypeValues.Medium9);
                sl.InsertTable(tbl);

                sl.AutoFitColumn(numeroColumnaInicio, numeroColumnaCuerpo);

                sl.SaveAs(Excel);
                sl.Dispose();
            }
        }

        private byte[] ProcesamientoGeneralImagen
            (string IdSesion, string ContenedorOrigen, Guid GuidNuevo, bool EsImagen, string html)
        {
            try
            {
                var contenedorEjecutables =
                    new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion
                        .ContenedorEjecutables);
                var conversor = EsImagen
                    ? Path.Combine(contenedorEjecutables, "wkhtmltoimage.exe")
                    : Path.Combine(contenedorEjecutables, "wkhtmltopdf.exe");

                html = TratamientoContenidoPdf(true, html);

                var archivoOrigen = Path.Combine(ContenedorOrigen, "Usuario_" + IdSesion + "_" + GuidNuevo + ".html");
                var archivoDestino = EsImagen
                    ? Path.Combine(ContenedorOrigen, "Usuario_" + IdSesion + "_" + GuidNuevo + ".jpg")
                    : Path.Combine(ContenedorOrigen, "Usuario_" + IdSesion + "_" + GuidNuevo + ".pdf");

                CreacionHTML(html, archivoOrigen);
                GeneracionPdf(archivoOrigen, archivoDestino, conversor);

                return LecturaDOC(archivoDestino);
            }
            catch (Exception ex)
            {
                RespuestaError(ex);
            }
            return null;
        }

        private string NombreSalida()
        {
            var nombreArchivo = ValorLLave("nameFichero");
            return string.IsNullOrEmpty(nombreArchivo) ? "Reporte" : nombreArchivo;
        }

        private string TratamientoContenidoPdf(bool ConEncabezado, string html)
        {
            html = string.IsNullOrEmpty(html) ? ValorLLave("inputHTML") : html;
            html = UnEscape(html);
            html = html.Replace("style=\"display:none;\"", "");
            html = html.Replace("none", "");
            if (ConEncabezado)
                html =
                    "<html><head><meta charset=\"utf-8\">" + 
                    "<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css\" integrity=\"sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7\" crossorigin=\"anonymous\">" +
                    "<link rel=\"stylesheet\" href=\"https://framework-gb.cdn.gob.mx/assets/styles/main.css\" > " +
            "</head>" +
                    html + "</html>";
            return html;
        }

        private string ValorLLave(string Llave)
        {
            return Request[Llave];
        }

        private byte[] LecturaDOC(string Destino)
        {
            return System.IO.File.ReadAllBytes(Destino);
        }

        private void RespuestaError(Exception Ex)
        {
            EscribirLog(new ExceptionContext(ControllerContext, Ex));
            var error = Ex.Message;
            error += "/n/r/n/r" + ErrorEjecucion;
            Response.Clear();
            Response.ClearHeaders();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AddHeader("Content-Type", "text/plain");
            Response.Charset = "";
            Response.AddHeader("Content-Disposition", "attachment;filename=Error.txt");
            Response.Write(error);
            Response.Flush();
            Response.End();
        }

        private void GeneracionPdf(string Origen, string Destino, string Ejecutable)
        {
            dynamic ConHeaderFooter = ValorLLave("ConHeaderFooter");
            try
            {
                ConHeaderFooter = int.Parse(ConHeaderFooter);
            }
            catch (Exception)
            {
                ConHeaderFooter = 0;
            }

            string dirBase = Path.GetDirectoryName(Ejecutable);
            string header = Path.Combine(dirBase, "header.html");
            string footer = Path.Combine(dirBase, "footer.html");

            string CadenaAuxiliar = ConHeaderFooter == 0 ? Origen + " " + Destino :
                " --header-html " + header + " --footer-html " + footer + " " + Origen + " " + Destino;

            using (var compiler = new Process
            {
                StartInfo =
                {
                    Arguments = CadenaAuxiliar,
                    FileName = Ejecutable,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            })
            {
                compiler.Start();

                ErrorEjecucion = "StandardError: " + compiler.StandardError.ReadToEnd();
                ErrorEjecucion = ErrorEjecucion + "/n/r/n/rStandardOutput: " + compiler.StandardOutput.ReadToEnd();

                compiler.WaitForExit();
            }
        }

        private void CreacionHTML(string Html, string Origen)
        {
            System.IO.File.WriteAllText(Origen, Html);
        }

        private void RespuestaXls(string NombreArchivo, byte[] BytesPdf)
        {
            if (BytesPdf == null) return;
            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Charset = "";
            Response.BufferOutput = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo);
            Response.AddHeader("Content-Length", BytesPdf.Length.ToString());
            Response.BinaryWrite(BytesPdf);
            Response.Flush();
            Response.End();
        }

        private void RespuestaJpg(string NombreArchivo, byte[] BytesPdf)
        {
            if (BytesPdf == null) return;
            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Charset = "";
            Response.BufferOutput = true;
            Response.ContentType = "image/JPEG";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo);
            Response.AddHeader("Content-Length", BytesPdf.Length.ToString());
            Response.BinaryWrite(BytesPdf);
            Response.Flush();
            Response.End();
        }

        private void RespuestaPdf(string NombreArchivo, byte[] BytesPdf)
        {
            if (BytesPdf == null) return;
            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Charset = "";
            Response.BufferOutput = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo);
            Response.AddHeader("Content-Length", BytesPdf.Length.ToString());
            Response.BinaryWrite(BytesPdf);
            Response.Flush();
            Response.End();
        }

        private void EliminacionTemporales(string Ruta, string Usuario)
        {
            var di = new DirectoryInfo(Ruta);
            foreach (var fi in di.GetFiles())
            {
                var fichero = fi.FullName;
                if (!fichero.Contains(Usuario)) continue;
                try
                {
                    System.IO.File.Delete(fichero);
                }
                catch
                {
                    // ignored
                }
            }
        }

        #endregion
    }
}