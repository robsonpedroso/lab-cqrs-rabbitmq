using CQRS.Tools.Utils.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CQRS.Tools.Utils.Components
{
    public class DiscriminatorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsClass;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var discriminatorProperty = objectType.GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttributes(typeof(JsonDiscriminatorAttribute), false).Length > 0);

                if (discriminatorProperty != null && discriminatorProperty.PropertyType.IsEnum)
                {
                    JObject jsonValue = JObject.Load(reader);

                    string stringValue = string.Empty;

                    if (jsonValue[discriminatorProperty.Name] != null)
                    {
                        stringValue = jsonValue[discriminatorProperty.Name].ToString();
                    }
                    else if (jsonValue[discriminatorProperty.Name.ToLower()] != null)
                    {
                        stringValue = jsonValue[discriminatorProperty.Name.ToLower()].ToString();
                    }

                    if (!stringValue.IsNullOrWhiteSpace())
                    {
                        var enumValue = Enum.Parse(discriminatorProperty.PropertyType, stringValue, true);

                        if (enumValue != null)
                        {
                            var attribute = enumValue.GetType()
                                .GetField(enumValue.ToString())
                                .GetCustomAttributes(typeof(JsonClassAttribute), false)
                                .FirstOrDefault() as JsonClassAttribute;

                            if (attribute != null)
                            {
                                var target = Activator.CreateInstance(attribute.ClassTypes.FirstOrDefault(t => t.Equals(objectType) || t.IsSubclassOf(objectType)));
                                serializer.Populate(jsonValue.CreateReader(), target);
                                return target;
                            }
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonDiscriminatorAttribute : Attribute { }

    public class JsonClassAttribute : Attribute
    {
        private Type[] classTypes;

        public Type[] ClassTypes { get { return classTypes; } }

        public JsonClassAttribute(params Type[] classTypes)
        {
            this.classTypes = classTypes;
        }
    }

}
