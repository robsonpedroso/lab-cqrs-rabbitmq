using System;
using System.Collections.Generic;
using System.IO;

namespace CQRS.Tools.Utils.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Check value objct is null.
        /// </summary>
        /// <param name="value">The object to be checked.</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        /// <summary>
        /// Check value objct is Database null.
        /// </summary>
        /// <param name="value">The object to be checked.</param>
        /// <returns>The boolean true or false.</returns>
        public static T IsDBNull<T>(this object value)
        {
            if (!Convert.IsDBNull(value))
                return (T)value;

            return default(T);
        }

        /// <summary>
        /// Check value objct is Database null.
        /// </summary>
        /// <param name="value">The object to be checked.</param>
        /// <param name="defaultValue">Optional default value is default(int).</param>
        /// <returns>The boolean true or false.</returns>
        public static T IsDBNull<T>(this object value, T defaultValue)
        {
            if (!Convert.IsDBNull(value))
                return (T)value;

            return defaultValue;
        }

        /// <summary>
        /// Transform object into integer data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultInt">Optional default value is default(int).</param>
        /// <returns>The integer value.</returns>
        public static int AsInt(this object item, int defaultInt = default(int))
        {
            if (item == null)
                return defaultInt;

            int result;

            if (item is Enum)
                result = Convert.ToInt32(item);
            else if (!int.TryParse(item.ToString(), out result))
                result = defaultInt;

            return result;
        }

        /// <summary>
        /// Transform object into integer data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultLong">Optional default value is default(int).</param>
        /// <returns>The integer value.</returns>
        public static long AsLong(this object item, long defaultLong = default(long))
        {
            if (item == null)
                return defaultLong;

            long result;
            if (!long.TryParse(item.ToString(), out result))
                return defaultLong;

            return result;
        }

        /// <summary>
        /// Transform object into double data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultDouble">Optional default value is default(double).</param>
        /// <returns>The double value.</returns>
        public static double AsDouble(this object item, double defaultDouble = default(double))
        {
            if (item == null)
                return defaultDouble;

            double result;
            if (!double.TryParse(item.ToString(), out result))
                return defaultDouble;

            return result;
        }

        /// <summary>
        /// Transform object into string data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultString">Optional default value is default(string).</param>
        /// <returns>The string value.</returns>
        public static string AsString(this object item, string defaultString = default(string))
        {
            if (item == null || item.Equals(System.DBNull.Value))
                return defaultString;

            return item.ToString().Trim();
        }

        /// <summary>
        /// Transform object into DateTime data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultDateTime">Optional default value is default(DateTime).</param>
        /// <returns>The DateTime value.</returns>
        public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            if (!DateTime.TryParse(item.ToString(), out result))
                return defaultDateTime;

            return result;
        }

        /// <summary>
        /// Transform object into decimal data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultDecimal">Optional default value is default(decimal).</param>
        /// <returns>The decimal value.</returns>
        public static decimal AsDecimal(this object item, decimal defaultDecimal = default(decimal))
        {
            if (item == null)
                return defaultDecimal;

            decimal result;
            if (!decimal.TryParse(item.ToString(), out result))
                return defaultDecimal;

            return result;
        }

        /// <summary>
        /// Transform object into decimal data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="culture">Optional culture. Default is en-US.</param>
        /// <param name="defaultDecimal">Optional default value is default(decimal).</param>
        /// <returns>The decimal value.</returns>
        public static decimal AsDecimal(this object item, System.Globalization.CultureInfo culture, decimal defaultDecimal = default(decimal))
        {
            if (item == null)
                return defaultDecimal;

            if (culture == null)
                culture = new System.Globalization.CultureInfo("en-US");

            decimal result;
            if (!decimal.TryParse(item.ToString(), System.Globalization.NumberStyles.Number, culture.NumberFormat, out result))
                return defaultDecimal;

            return result;
        }

        /// <summary>
        /// Transform object into bool data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultBool">Optional default value is default(bool).</param>
        /// <returns>The bool value.</returns>
        public static bool AsBool(this object item, bool defaultBool = default(bool))
        {
            if (item == null)
                return defaultBool;

            return new List<string>() { "yes", "y", "true", "1" }.Contains(item.ToString().ToLower());
        }

        /// <summary>
        /// Transform object into Guid data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <returns>The Guid value.</returns>
        public static Guid AsGuid(this object item)
        {
            try { return new Guid(item.ToString()); }
            catch { return Guid.Empty; }
        }

        /// <summary>
        /// Transform string into byte array.
        /// </summary>
        /// <param name="value">The object to be transformed.</param>
        /// <returns>The transformed byte array.</returns>
        public static byte[] AsByteArray(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return Convert.FromBase64String(value);
        }

        /// <summary>
        /// Transform object into base64 string.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="getEncodingByes">Get Bytes from String using Enconding.</param>
        /// <returns>The base64 string value.</returns>
        public static string AsBase64String(this object item, bool getEncodingByes)
        {
            byte[] bytesItem = null;

            if (item == null)
                return null;

            if (getEncodingByes)
                bytesItem = System.Text.UTF8Encoding.UTF8.GetBytes(item.ToString());
            else
                bytesItem = (byte[])item;

            return Convert.ToBase64String(bytesItem);
        }

        /// <summary>
        /// Transform Stream into byte array.
        /// </summary>
        /// <param name="inputStream">The object to be transformed.</param>
        /// <returns>The transformed byte array.</returns>
        public static Byte[] AsByte(this Stream inputStream)
        {
            byte[] data;

            using (var stream = inputStream)
            {
                var memoryStream = stream as MemoryStream;

                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }

                data = memoryStream.ToArray();
            }

            return data;
        }

        /// <summary>
        /// Transform object into base64 string.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <returns>The base64 string value.</returns>
        public static string AsBase64String(this object item)
        {
            return AsBase64String(item, false);
        }

        /// <summary>
        /// For Each in IList, Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            foreach (T t in list)
                action(t);
        }

        /// <summary>
        /// For Each in IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T t in list)
                action(t);
        }
    }
}
