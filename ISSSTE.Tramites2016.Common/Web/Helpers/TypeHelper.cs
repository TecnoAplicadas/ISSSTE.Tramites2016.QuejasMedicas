using System;

namespace ISSSTE.Tramites2016.Common.Web.Helpers
{
    /// <summary>
    ///     Contains methods that help identify the type of a object
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        ///     Checks if the type is any kind of character based type
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsString(Type type)
        {
            return type == typeof(string) || type == typeof(char) || type == typeof(char?);
        }

        /// <summary>
        ///     Checks if the type is any float point number
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsFloat(Type type)
        {
            return type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?);
        }

        /// <summary>
        ///     Checks if the type is a decimal number
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsDecimal(Type type)
        {
            return type == typeof(decimal) || type == typeof(decimal?);
        }


        /// <summary>
        ///     Checks if the type is a boolean
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsBoolean(Type type)
        {
            return type == typeof(bool) || type == typeof(bool?);
        }

        /// <summary>
        ///     Checks if the type is a Date
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsDate(Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }
    }
}