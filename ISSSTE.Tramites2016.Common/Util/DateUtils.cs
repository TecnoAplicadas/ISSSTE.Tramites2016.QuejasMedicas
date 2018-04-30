#region

using System;

#endregion

namespace ISSSTE.Tramites2015.Common.Util
{
    /// <summary>
    /// Contiene métodos para mejo de fechas
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Cuenta los días hábiles entre dos fechas, tomando en cuenta una lista de días inhabiles dicionales
        /// </summary>
        /// <param name="startDate">Fecha desde la cual iniciar el conteo</param>
        /// <param name="endDate">Fecha hastala cual contar los días</param>
        /// <param name="holydays">Lista de días inhábiles</param>
        /// <returns>Número de días hábiles entre las fechas</returns>
        public static int CountWorkDays(DateTime startDate, DateTime endDate, params DateTime[] holydays)
        {
            var numberOfDays = CountDays(startDate, endDate, holydays);

            var numberOfSaturdays = (numberOfDays + Convert.ToInt32(startDate.DayOfWeek)) / 7;

            numberOfDays = numberOfDays - 2 * numberOfSaturdays
                           - (startDate.DayOfWeek == DayOfWeek.Sunday ? 1 : 0)
                           + (endDate.DayOfWeek == DayOfWeek.Saturday ? 1 : 0);

            return numberOfDays;
        }

        /// <summary>
        /// Cuenta los días entre dos fechas, tomando en cuenta una lista de días inhabiles dicionales
        /// </summary>
        /// <param name="startDate">Fecha desde la cual iniciar el conteo</param>
        /// <param name="endDate">Fecha hastala cual contar los días</param>
        /// <param name="holydays">Lista de días inhábiles</param>
        /// <returns>Número de días entre las fechas</returns>
        public static int CountDays(DateTime startDate, DateTime endDate, params DateTime[] holydays)
        {
            var numberOfDays = (int)Math.Floor((endDate - startDate).TotalDays);

            foreach (var actualHolyday in holydays)
            {
                if (startDate <= actualHolyday.Date && actualHolyday.Date <= endDate)
                    --numberOfDays;
            }

            return numberOfDays;
        }
    }
}