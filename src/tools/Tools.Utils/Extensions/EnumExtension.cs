using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CQRS.Tools.Utils.Extensions
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string value, bool? ignoreCase = null)
        {
            if (value.IsNullOrEmpty()) return default(T);

            if (!ignoreCase.HasValue)
                return (T)Enum.Parse(typeof(T), value);

            return (T)Enum.Parse(typeof(T), value, ignoreCase.Value);
        }

        public static T ToEnum<T>(this object value, bool? ignoreCase = null)
        {
            if (value.IsNull()) return default(T);

            return value.ToString().ToEnum<T>();
        }

        public static int ToValue<T>(this string value)
        {
            return Convert.ToInt32(Enum.Parse(typeof(T), value));
        }

        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string ToLower(this Enum value)
        {
            return value.AsString().ToLower();
        }

        public static IEnumerable<T> GetValues<T>()
        {
            if (typeof(T).IsEnum)
                return Enum.GetValues(typeof(T)).Cast<T>();

            return default(IEnumerable<T>);
        }
    }
}
