using Tools.Utils.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Tools.Utils.Components
{
    public abstract class PropertiesConverter : JsonConverter
    {
        private IEnumerable<string> properties;
        private bool allow;

        public PropertiesConverter(string propertiesString, bool allow = true)
        {
            this.allow = allow;

            if (!propertiesString.IsNullOrWhiteSpace())
                this.properties = propertiesString.Replace(" ", "").Split(',');
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsClass;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueType = value.GetType();

            if (valueType.IsConstructedGenericType)
                valueType = valueType.GetGenericArguments().FirstOrDefault();

            var propertiesToAllow = properties.Select(name =>
            {
                if (value.IsNull())
                    return name;

                var property = valueType.GetProperty(name);

                if (property.IsNull() || property.DeclaringType.IsNull())
                    return name;

                return string.Format("{0}.{1}", property.DeclaringType, name);
            });

            var resolver = allow
                ? new AllowPropertyContractResolver(propertiesToAllow.ToArray()) as PropertyContractResolver
                : new IgnorePropertyContractResolver(propertiesToAllow.ToArray()) as PropertyContractResolver;

            var serializerNew = new JsonSerializer
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.None,
                FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
                FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ContractResolver = resolver
            };

            serializerNew.Serialize(writer, value);
        }
    }
}
