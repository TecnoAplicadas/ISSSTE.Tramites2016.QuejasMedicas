using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using System;
using static System.DateTime;

namespace ISSSTE.Tramites2016.QuejasMedicas.Helpers
{
    public class FechaConversion
    {
        public static DatesDTO GetDates(string dates)
        {
            DateTime? fechaInicio= null;
            DateTime? fechaFin = null;

            var _FechaConversion = new FechaConversion();
            _FechaConversion.ConversionFecha(dates, ref fechaInicio, ref fechaFin);

            return new DatesDTO
            {
                FechaFin = fechaFin,
                FechaInicio = fechaInicio
            };
        }

        private void ConversionFecha(string dates, ref DateTime? fechaInicio, ref DateTime? fechaFin)
        {
            try
            {
                var datesArray = dates.Split('-');

                if (datesArray.Length == 2)
                {
                    fechaInicio= Convert.ToDateTime(datesArray[0]);
                    fechaFin = Convert.ToDateTime(datesArray[1]);
                }

                fechaFin = Convert.ToDateTime(fechaFin.Value.ToShortDateString() + " 12:00:00 PM");
            }
            catch (Exception)
            {
                fechaInicio = null;
                fechaFin = null;
            }
        }

        public static long ConvertDateToTInMS(DateTime date)
        {
            var unixTime = date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) unixTime.TotalMilliseconds;
        }
    }



}