using Tools.Utils.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tools.Utils.Components
{
    /// <summary>
    /// Instructs the Newtonsoft.Json.JsonSerializer to use the specified Newtonsoft.Json.JsonConverter when serializing the member or class.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    public class HandlerConverter<T> : JsonConverter where T : class
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(T));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (token.Type == JTokenType.Object || token.Type == JTokenType.Array)
                return token.ToObject<T>();

            return null;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    /// <summary>
    /// Used by Newtonsoft.Json.JsonSerializer to resolves a Newtonsoft.Json.Serialization.JsonContract for a given System.Type.
    /// </summary>    
    public abstract class PropertyContractResolver : ProxyContractResolver
    {
        private bool allow;
        private IEnumerable<string> propertyNames;

        /// <summary>
        /// Used by Newtonsoft.Json.JsonSerializer to resolves a Newtonsoft.Json.Serialization.JsonContract for a given System.Type.
        /// </summary>
        /// <param name="allow"></param>
        /// /// <param name="propertyNames"></param>
        public PropertyContractResolver(bool allow, params string[] propertyNames)
        {
            this.allow = allow;
            this.propertyNames = propertyNames;
        }

        /// <summary>
        /// Creates a Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.MemberInfo.
        /// </summary>
        /// <param name="member">The member to create a Newtonsoft.Json.Serialization.JsonProperty for.</param>
        /// <param name="memberSerialization">The member's parent Newtonsoft.Json.MemberSerialization.</param>
        /// <returns>A created Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.MemberInfo.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (!propertyNames.IsNull() || propertyNames.Count() > 0)
            {
                var propertyName = string.Format("{0}.{1}", member.DeclaringType, member.Name);

                if (allow)
                {
                    if (!propertyNames.Contains(propertyName))
                        return null;
                }
                else if (propertyNames.Contains(propertyName))
                {
                    return null;
                }
            }

            return base.CreateProperty(member, memberSerialization);
        }
    }

    public class IgnorePropertyContractResolver : PropertyContractResolver
    {
        public IgnorePropertyContractResolver(params string[] propertyNames) : base(false, propertyNames) { }
    }

    public class AllowPropertyContractResolver : PropertyContractResolver
    {
        public AllowPropertyContractResolver(params string[] propertyNames) : base(true, propertyNames) { }
    }
}
