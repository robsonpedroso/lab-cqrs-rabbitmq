using Jose;
using Newtonsoft.Json;

namespace Tools.Utils.Components
{
    public class JwtNewtonsoftMapper : IJsonMapper
    {
        private JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            Formatting = Newtonsoft.Json.Formatting.None,
            FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
            FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double,
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
            DateFormatString = "yyyy-MM-ddTHH:mm:ssZ"
        };

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
