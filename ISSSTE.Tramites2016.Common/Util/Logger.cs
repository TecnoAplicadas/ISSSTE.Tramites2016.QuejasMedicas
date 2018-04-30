#region

using System;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Elmah;
using Newtonsoft.Json;

#endregion

namespace ISSSTE.Tramites2016.Common.Util
{
    /// <summary>
    /// Implementación de <see cref="ILogger"/>
    /// </summary>
    public class Logger : ILogger
    {
        #region Constructor

        /// <summary>
        /// Contructor de la clase.
        /// Utiliza el app.config para configurar el log
        /// </summary>
        public Logger()
        {
            try
            {


                var logSource = ConfigurationManager.AppSettings["LogSource"];
                var logName = ConfigurationManager.AppSettings["LogName"];
                var priority = ConfigurationManager.AppSettings["LogPriority"];

                _eventLog = new EventLog();
                _priority = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), priority);

                if (!EventLog.SourceExists(logSource))
                    EventLog.CreateEventSource(logSource, logName);

                _eventLog.Source = logSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// EventLog en el cual escribir las entradas
        /// </summary>
        private readonly EventLog _eventLog;
        /// <summary>
        /// Prioridad con la cual escribir las entradas
        /// </summary>
        private readonly EventLogEntryType _priority;

        #endregion

        #region ILogger Implementation

        /// <summary>
        /// Escribe una entrada en el log
        /// </summary>
        /// <param name="message">Texo del mnsaje</param>
        /// <param name="type">Tipo de entarda</param>
        public void WriteEntry(string message, EventLogEntryType type)
        {
            if ((int)type <= (int)_priority)
                _eventLog.WriteEntry(message, type);
        }

        /// <summary>
        /// Escribe una excepción en el log
        /// </summary>
        /// <param name="exception">Excepción a serializar</param>
        /// <param name="message">Información adicional a escribir</param>
        public void WriteEntry(Exception exception, string message = null)
        {
            try
            {
                ErrorSignal.FromCurrentContext().Raise(exception);
            }
            catch (ArgumentNullException)
            {
                //Ésta excepción se lanza si no hay contexto MVC o Web Api sobre el cual lanzar la excepción
                //Puede pasar cuando se manda a escribir la entrada asincronamiente
            }

            var logMessage = "";

            if (!String.IsNullOrEmpty(message))
                logMessage = message + "\n\n";

            if (exception is DbEntityValidationException)
            {
                logMessage += "Mensaje: \n";
                logMessage +=
                    String.Concat(
                        (exception as DbEntityValidationException).EntityValidationErrors.SelectMany(
                            ev => ev.ValidationErrors).Select(ve => ve.ErrorMessage + "\n"));
                logMessage += "\nStackTrace: \n";
                logMessage += exception.StackTrace;
            }
            else
                logMessage += JsonConvert.SerializeObject(exception,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            _eventLog.WriteEntry(logMessage, EventLogEntryType.Error);
        }


        /// <summary>
        /// Asíncronamenete escribe una entrada en el log
        /// </summary>
        /// <param name="message">Texo del mnsaje</param>
        /// <param name="type">Tipo de entarda</param>
        public async Task WriteEntryAsync(string message, EventLogEntryType type)
        {
            await Task.Run(() => { WriteEntry(message, type); });
        }

        /// <summary>
        /// Asíncronamenete escribe una excepción en el log
        /// </summary>
        /// <param name="exception">Excepción a serializar</param>
        /// <param name="message">Información adicional a escribir</param>
        public async Task WriteEntryAsync(Exception exception, string message = null)
        {
            await Task.Run(() => { WriteEntry(exception, message); });
        }

        #endregion
    }
}