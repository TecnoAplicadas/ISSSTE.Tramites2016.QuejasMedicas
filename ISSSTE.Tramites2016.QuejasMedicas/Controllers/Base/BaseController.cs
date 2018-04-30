using System;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    public class BaseController : Controller
    {


        protected virtual async Task<R> HandleOperationExecutionAsync<R>(Func<Task<R>> operationBody)
        where R : ActionResult
        {
            ActionResult response = null;

            try
            {
                return await operationBody();
            }
            catch (Exception ex)
            {
                //LogException(ex);

                throw;
            }
        }

        /// <summary>
        ///     Ejecuta código y majea excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Código a ejecutar</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected virtual R HandleOperationExecution<R>(Func<R> operationBody) where R : ActionResult
        {
            var validationTask = HandleOperationExecutionAsync(async () => operationBody());

            validationTask.Wait();

            return validationTask.Result;
        }

        public static string UnEscape(string HexString)
        {
            string[] escapados =
            {
                "%20", "%21", "%22", "%23", "%24", "%25", "%26", "%27", "%28", "%29",
                "%2A", "%2B", "%2C", "%2D", "%2E", "%2F", "%30", "%31", "%32", "%33",
                "%34", "%35", "%36", "%37", "%38", "%39", "%3A", "%3B", "%3C", "%3D", "%3E",
                "%3F", "%40", "%41", "%42", "%43", "%44", "%45", "%46", "%47", "%48", "%49",
                "%4A", "%4B", "%4C", "%4D", "%4E", "%4F", "%50", "%51", "%52", "%53", "%54",
                "%55", "%56", "%57", "%58", "%59", "%5A", "%5B", "%5C", "%5D", "%5E", "%5F",
                "%60", "%61", "%62", "%63", "%64", "%65", "%66", "%67", "%68", "%69", "%6A",
                "%6B", "%6C", "%6D", "%6E", "%6F", "%70", "%71", "%72", "%73", "%74", "%75",
                "%76", "%77", "%78", "%79", "%7A", "%7B", "%7C", "%7D", "%7E", "%7F", "%80",
                "%82", "%85", "%96", "%C1", "%C9", "%CD", "%D3", "%D6", "%DA", "%DC", "%E1",
                "%E9", "%EB", "%ED", "%F3", "%F6", "%FA", "%FC", "%0A", "%BF", "%D1"
            };

            string[] convertidos =
            {
                " ", "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-",
                ".", "/", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ":", ";", "<",
                "=", ">", "?", "@", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
                "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "[", "\\", "]", "^", "_", "`", "a", "b", "c", "d", "e", "f", "g", "h", "i",
                "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x",
                "y", "z", "{", "|", "}", "~", "", "`", ",", "...", "-", "Á", "É", "Í", "Ó",
                "Ö", "Ú", "Ü", "á", "é", "ë", "í", "ó", "ö", "ú", "ü", "", " ", "Ñ"
            };

            var indice = 0;
            foreach (var elem in escapados)
            {
                HexString = HexString.Replace(elem, convertidos[indice]);
                indice++;
            }
            HexString = HexString.Replace("%F1", "ñ");

            var contenedorImagenes = "";
            try
            {
                var ImagenObligatoria = "~/Images/Promovente/error.png";
                contenedorImagenes = HostingEnvironment.MapPath(ImagenObligatoria);
                contenedorImagenes = Path.GetDirectoryName(contenedorImagenes);
            }
            catch (Exception)
            {
                // ignored
            }
            HexString = HexString.Replace("../Images/Promovente/", contenedorImagenes + "\\");

            return HexString;
        }

        /// <summary>
        ///     Conversión caracteres ASCII a HEX
        /// </summary>
        /// <param name="HexString"></param>
        /// <returns></returns>
        public static string ConvertAsciiToHex(string HexString)
        {
            string[] escapados =
            {
                "%25", "%21", "%23", "%24", "%26", "%28", "%29", "%3D", "%3F", "%27",
                "%A1", "%BF", "%A8", "%B4", "%7E", "%5B", "%7B", "%5E", "%5D", "%7D",
                "%60", "%2C", "%3B", "%3A", "%22", "%3C", "%3E", "%2F"
            };

            string[] convertidos =
            {
                "%", "!", "#", "$", "&", "(", ")", "=", "?", "'",
                "¡", "¿", "¨", "´", "~", "[", "{", "^", "]", "}",
                "`", ",", ";", ":", "\"", "<", ">", "/"
            };

            var indice = 0;
            foreach (var elem in convertidos)
            {
                HexString = HexString.Replace(elem, escapados[indice]);
                indice++;
            }
            return HexString;
        }

        protected override void OnException(ExceptionContext FilterContext)
        {
            if (FilterContext.ExceptionHandled) return;
            var rutaLog = new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaLog);
            if (!string.IsNullOrEmpty(rutaLog))
            {
                FilterContext.ExceptionHandled = true;

                EscribirLog(FilterContext);

                FilterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.ErrorGeneral]
                    }
                };
            }
            base.OnException(FilterContext);
        }

        public static void DebugLog(string Mensaje)
        {
            Mensaje = "" + DateTime.Now + " - " + Mensaje;
            var rutaLog = new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaLog);
            Logger.DebugLog(Mensaje, rutaLog);
        }

        public static void EscribirLog(ExceptionContext FilterContext)
        {
            var rutaLog = new ConfiguracionService().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaLog);
            var controller = FilterContext.RouteData.Values["controller"].ToString();
            var action = FilterContext.RouteData.Values["action"].ToString();
            var ex = FilterContext.Exception;
            var validacionesModelo = GenericoDAO.DB_Validacion(ex);

            var queryString = FilterContext.RequestContext.HttpContext.Request.QueryString.ToString();
            Logger.EscribirLog(ex, rutaLog, validacionesModelo, controller, action, queryString);
        }

        #region Control de sesiones

        private const string IdCaptcha = "Captcha";
        private const string IdUsuario = "IdUsuario";
        private const string UserName = "UserName";
        private const string ClaveName = "ClaveName";
        private const string CatTipoTramiteId = "CatTipoTramiteId";
        private const string FechaLimite = "FechaLimite";

        public void DestroyKeySesion(string Llave)
        {
            try
            {
                Session.Remove(Llave);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public string IdUserName
        {
            get { return IdCadenaSesion(UserName); }
            set { Session[UserName] = value; }
        }

        public string IdClaveName
        {
            get { return IdCadenaSesion(ClaveName); }
            set { Session[ClaveName] = value; }
        }

        public string IdFechaLimite
        {
            get { return IdCadenaSesion(FechaLimite); }
            set { Session[FechaLimite] = value; }
        }

        public int IdSesionCaptcha
        {
            get { return IdSesion(IdCaptcha); }
            set { Session[IdCaptcha] = value; }
        }

        public int IdSesionUsuario
        {
            get { return IdSesion(IdUsuario); }
            set { Session[IdUsuario] = value; }
        }

        public int IdSesionTipoTramite
        {
            get { return IdSesion(CatTipoTramiteId); }
            set { Session[CatTipoTramiteId] = value; }
        }

        private int IdSesion(string Llave)
        {
            try
            {
                return (int) Session[Llave];
            }
            catch
            {
                return 0;
            }
        }

        private string IdCadenaSesion(string Llave)
        {
            if (Llave == null) throw new ArgumentNullException(nameof(Llave));
            return (string) Session[Llave];
        }

        #endregion
    }
}