using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Base
{
    /// <summary>
    ///     Controlador base que contiene metodos auxiliares para todos los controladores Web API
    /// </summary>
    public abstract class BaseApiController : ApiController
    {
        #region Fields

        /// <summary>
        ///     Clase utilizada para logear
        /// </summary>
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="logger">Objeto a utilizar para loguear información</param>
        public BaseApiController(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Crea una respuesta cuyo contenido es una cadena
        /// </summary>
        /// <param name="statusCode">Estatus de la respuesta</param>
        /// <param name="message">Contenido de la respuesta</param>
        /// <returns>Respueta</returns>
        protected virtual HttpResponseMessage CreateStringResponseMessage(HttpStatusCode statusCode, string message)
        {
            var responseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(message)
            };

            return responseMessage;
        }

        /// <summary>
        ///     Ejecuta código asíncrono y maneja excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Código a ejecutar</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected virtual async Task<HttpResponseMessage> HandleOperationExecutionAsync(
            Func<Task<HttpResponseMessage>> operationBody)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await operationBody();
            }
            catch (Exception ex)
           {
                LogException(ex);
#if DEBUG

                if (ex is DbEntityValidationException)
                    response = CreateStringResponseMessage(HttpStatusCode.InternalServerError,
                        String.Concat(
                            (ex as DbEntityValidationException).EntityValidationErrors.SelectMany(
                                ev => ev.ValidationErrors).Select(ve => ve.ErrorMessage + "\n")));
                else
                    response = CreateStringResponseMessage(HttpStatusCode.InternalServerError,
                        JsonConvert.SerializeObject(ex));
#else
                response = CreateStringResponseMessage(HttpStatusCode.InternalServerError, 
                    String.Format("ID de error: {0}", DateTime.Now.ToString("yyyyMMddTHHmm")));
#endif
            }

            return response;
        }

        /// <summary>
        ///     Ejecuta código y maneja excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Código a ejecutar</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected virtual HttpResponseMessage HandleOperationExecution(Func<HttpResponseMessage> operationBody)
        {
            var handleoperationTask = HandleOperationExecutionAsync(async () => operationBody());

            handleoperationTask.Wait();

            return handleoperationTask.Result;
        }

        /// <summary>
        ///     Ejecuta código asíncrono con el contenido multipart enviado a en la petición y maneja excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected async Task<HttpResponseMessage> HandleMultipartOperationExecutionAsync(
            Func<HttpPostedData, Task<HttpResponseMessage>> operationBody)
        {
            return await HandleOperationExecutionAsync(async () =>
            {
                HttpResponseMessage response = null;

                var multipartData = await Request.Content.ParseMultipartAsync();//.ParseMultipartAsync();

                response = await operationBody(multipartData);

                return response;
            });
        }

        /// <summary>
        ///     Ejecuta código con el contenido multipart enviado a en la petición y maneja excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Cuerpo a ejecutar si se valido la petición</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
        protected HttpResponseMessage HandleMultipartOperationExecution(
            Func<HttpPostedData, HttpResponseMessage> operationBody)
        {
            var handleoperationTask =
                HandleMultipartOperationExecutionAsync(async multipartData => operationBody(multipartData));

            handleoperationTask.Wait();

            return handleoperationTask.Result;
        }

        /// <summary>
        ///     Escribe al log y a ELMAH una excepción
        /// </summary>
        /// <param name="ex">Excepción a escribir al log</param>
        protected virtual void LogException(Exception ex)
        {
            //_logger.WriteEntry(ex);
            throw ex;
          
        }

        #endregion
    }
}
