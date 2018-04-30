#region

using System;
using System.Diagnostics;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Common.Util
{
    /// <summary>
    /// Contiene métodos utilizado para loguear información
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Escribe una entrada en el log
        /// </summary>
        /// <param name="message">Texo del mnsaje</param>
        /// <param name="type">Tipo de entarda</param>
        void WriteEntry(string message, EventLogEntryType type);

        /// <summary>
        /// Escribe una excepción en el log
        /// </summary>
        /// <param name="exception">Excepción a serializar</param>
        /// <param name="message">Información adicional a escribir</param>
        void WriteEntry(Exception exception, string message = null);

        /// <summary>
        /// Asíncronamenete escribe una entrada en el log
        /// </summary>
        /// <param name="message">Texo del mnsaje</param>
        /// <param name="type">Tipo de entarda</param>
        Task WriteEntryAsync(string message, EventLogEntryType type);

        /// <summary>
        /// Asíncronamenete escribe una excepción en el log
        /// </summary>
        /// <param name="exception">Excepción a serializar</param>
        /// <param name="message">Información adicional a escribir</param>
        Task WriteEntryAsync(Exception exception, string message = null);
    }
}