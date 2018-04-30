#region

using System;
using System.Collections.Generic;
using System.Configuration;
using ISSSTE.Tramites2015.Common.Model;

#endregion

namespace ISSSTE.Tramites2015.Common.Util.Estancias
{
    public static class EstanciasUtils
    {
        private static readonly int YEARS_MAX = Convert.ToInt32(ConfigurationManager.AppSettings["YearsMax"]);
        private static readonly int YEARS_MIN = Convert.ToInt32(ConfigurationManager.AppSettings["YearsMin"]);
        private static readonly int DAY_LIMIT = Convert.ToInt32(ConfigurationManager.AppSettings["DaysMax"]);
        private static readonly int MONTH_MIN = Convert.ToInt32(ConfigurationManager.AppSettings["MonthMin"]);
        private static readonly int MONTH_MAX = Convert.ToInt32(ConfigurationManager.AppSettings["MonthMax"]);

        private static readonly int MAX_MONTHS_BEFORE_NEXT_YEAR =
            Convert.ToInt32(ConfigurationManager.AppSettings["MaxMonthsBeforeNextYear"]);

        public static bool ValidateAge(DateTimeSpan age)
        {
            var canEnroll = true;

            if (age.Years <= YEARS_MAX)
            {
                if (age.Years == YEARS_MAX && (age.Months >= MONTH_MIN || age.Days >= DAY_LIMIT))
                    canEnroll = false;
                if (age.Years == YEARS_MIN && age.Months < MONTH_MAX)
                    canEnroll = false;
            }
            else
                canEnroll = false;

            return canEnroll;
        }

        public static int GetStratum(DateTimeSpan kidSpan, List<StratumModel> stratums)
        {
            var stratumId = 0;
            var years = kidSpan.Years;
            var months = kidSpan.Months;
            var days = kidSpan.Days;

            var isLimitMonth = false;

            foreach (var stratum in stratums)
            {
                isLimitMonth = false;

                if (years >= stratum.StartAge && years <= stratum.EndAge)
                {
                    if (months >= stratum.StartMonths && months <= stratum.EndMonths)
                    {
                        isLimitMonth = months == stratum.EndMonths;
                        if (days >= stratum.StartDays && days <= stratum.EndDays)
                        {
                            stratumId = stratum.StratumId;
                            break;
                        }
                        if (stratum.EndMonths == MAX_MONTHS_BEFORE_NEXT_YEAR)
                        {
                            months = 0;
                            years++;
                        }
                        else
                            months++;
                    }
                    else
                    {
                        if (stratum.StartMonths == 0 && stratum.EndMonths == 0)
                        {
                            stratumId = stratum.StratumId;
                            break;
                        }

                        if (stratum.EndMonths > 0)
                            continue;
                    }
                }
                else
                    continue;
            }

            return stratumId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stratum"></param>
        /// <returns></returns>
        public static StratumEnum NextStratumEnum(StratumEnum stratum)
        {
            if (stratum == StratumEnum.LactantesA)
            {
                return StratumEnum.LactantesB;
            }
            if (stratum == StratumEnum.LactantesB)
            {
                return StratumEnum.LactantesC;
            }
            if (stratum == StratumEnum.LactantesC)
            {
                return StratumEnum.MaternalA;
            }
            if (stratum == StratumEnum.MaternalA)
            {
                return StratumEnum.MaternalB;
            }
            if (stratum == StratumEnum.MaternalB)
            {
                return StratumEnum.Preescolar1;
            }
            if (stratum == StratumEnum.Preescolar1)
            {
                return StratumEnum.Preescolar2;
            }
            if (stratum == StratumEnum.Preescolar2)
            {
                return StratumEnum.Preescolar3;
            }
            if (stratum == StratumEnum.Preescolar3)
            {
                return StratumEnum.WihtOutStratum;
            }
            return StratumEnum.WihtOutStratum;
        }
    }

}
