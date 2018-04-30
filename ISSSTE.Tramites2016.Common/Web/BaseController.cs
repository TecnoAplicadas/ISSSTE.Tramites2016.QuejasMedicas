#region

using ISSSTE.Tramites2015.Common.Util;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    ///     Controlador base que contiene metodos auxiliares para todos los controladores MVC
    /// </summary>
    public abstract class BaseController : Controller
    {
        #region Fields

        /// <summary>
        ///     Clase utilizada para logear
        /// </summary>
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Ejecuta código asíncrono y majea excepciones no controladas
        /// </summary>
        /// <param name="operationBody">Código a ejecutar</param>
        /// <returns>Resultado del cuerpo a ejecutar</returns>
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
                LogException(ex);

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

        /// <summary>
        ///     Escribe al log y a ELMAH una excepción
        /// </summary>
        /// <param name="ex">Excepción a escribir al log</param>
        protected virtual void LogException(Exception ex)
        {
            _logger.WriteEntry(ex);
        }

        #endregion
    }
}