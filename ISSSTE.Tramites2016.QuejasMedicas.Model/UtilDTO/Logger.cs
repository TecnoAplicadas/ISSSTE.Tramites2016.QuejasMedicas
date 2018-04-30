using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class Logger
    {
        public static string DB_Data(Exception Ex)
        {
            var entityValidationErrors = new StringBuilder();
            if (Ex.Data.Count > 0)
                foreach (DictionaryEntry de in Ex.Data)
                {
                    entityValidationErrors.Append("Llave: " + de.Key + ", ");
                    entityValidationErrors.Append("Valor: " + de.Value);
                    entityValidationErrors.AppendLine();
                }
            return entityValidationErrors.ToString();
        }

        public static void EscribirLog(Exception FilterContext, string RutaLog, string ValidacionesModelo,
            string Controller = null, string Action = null, string QueryString = null)
        {
            var logger = new Logger();
            var hayError = logger.ExisteRutaLog(RutaLog);

            if (hayError) return;

            using (var w = File.AppendText(RutaLog))
            {
                w.WriteLine("*************************************************************************************");
                w.WriteLine("*************************************************************************************");
                w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - EXCEPCIÓN");
                if (!string.IsNullOrEmpty(Controller))
                {
                    w.WriteLine("Controller: " + Controller);
                    w.WriteLine("Action: " + Action);
                }
                w.WriteLine("-------------------------------------------------------------------------------------");
                w.WriteLine("Type: " + FilterContext.GetType());
                w.WriteLine("Message: " + FilterContext.Message);
                w.WriteLine("Data: " + FilterContext.Data);
                w.WriteLine("Source: " + FilterContext.Source);
                w.WriteLine("TargetSite: " + FilterContext.TargetSite);
                w.WriteLine("StackTrace: " + logger.GetTraza(FilterContext));
                w.WriteLine("-------------------------------------------------------------------------------------");
                w.WriteLine("EntityValidationErrors: " + ValidacionesModelo);
                w.WriteLine("Data: " + DB_Data(FilterContext));
                w.WriteLine("QueryString: " + QueryString);
            }
        }

        private string GetTraza(Exception FilterContext)
        {
            var Patron = "--- Fin del seguimiento de la pila";
            var traza = FilterContext.StackTrace;
            return Regex.Split(traza, Patron, RegexOptions.IgnoreCase)[0];
        }

        private bool ExisteRutaLog(string RutaLog)
        {
            var hayError = false;

            try
            {
                if (!File.Exists(RutaLog))
                    File.Create(RutaLog).Close();
            }
            catch
            {
                hayError = true;
            }

            return hayError;
        }

        public static void DebugLog(string Mensaje, string RutaLog)
        {
            var logger = new Logger();
            var hayError = logger.ExisteRutaLog(RutaLog);
            var llave = ConfigurationManager.AppSettings["WithLoggerDebug"];
            var conDebug = !string.IsNullOrEmpty(llave) && Convert.ToBoolean(llave);

            if (hayError || !conDebug) return;

            using (var w = File.AppendText(RutaLog))
            {
                w.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                w.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                w.WriteLine("Debug: " + Mensaje);
                w.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                w.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            }
        }
    }
}