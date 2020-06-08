using Tools.Utils.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tools.Utils.Extensions
{
    public static class SerializeExtensions
    {
        #region "  XML Serialization  "

        /// <summary>
        /// Serialize object into XML
        /// </summary>
        /// <param name="obj">The object to be transformed</param>
        /// <param name="type">The type to be transformed</param>
        /// <returns>The string in XML</returns>
        public static string Serialize(this object obj, Type type = null)
        {
            if (obj.IsNull()) return null;

            var xml = string.Empty;
            var typeObj = obj.GetType();

            if (!type.IsNull())
                typeObj = type;

            var settings = new XmlWriterSettings()
            {
                Encoding = Encoding.GetEncoding("ISO-8859-1")
            };

            var ns = new XmlSerializerNamespaces();

            ns.Add("", "");

            var serializer = new XmlSerializer(typeObj);

            using (var ms = new MemoryStream())
            {
                using (var writer = XmlTextWriter.Create(ms, settings))
                    serializer.Serialize(writer, obj, ns);

                xml = Encoding.GetEncoding("ISO-8859-1").GetString(ms.ToArray());
            }

            return xml;
        }

        /// <summary>
        /// Serialize object into XML specified
        /// </summary>
        /// <typeparam name="T">The type to be transformed</typeparam>
        /// <param name="obj">The object to be transformed</param>
        /// <returns>The string in XML</returns>
        public static string Serialize<T>(this object obj)
        {
            return Serialize(obj, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeUTF8(this object obj)
        {
            if (obj.IsNull())
                return string.Empty;

            string utf8String;
            var serializer = new XmlSerializer(obj.GetType());

            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, obj);
                utf8String = writer.ToString();
            }

            return utf8String;
        }

        /// <summary>
        /// Deserialize XML string into object specified
        /// </summary>
        /// <typeparam name="T">The type to be transformed</typeparam>
        /// <param name="xml">The string in XML</param>
        /// <returns>The object to be transformed</returns>
        public static T Deserialize<T>(this string xml) where T : class
        {
            if (xml.IsNullOrEmpty()) return null;

            var obj = new object();

            var xs = new XmlSerializer(typeof(T));

            try
            {
                obj = xs.Deserialize(new XmlTextReader(xml, XmlNodeType.Document, null));
            }
            catch (Exception)
            {
                obj = null;
            }

            return obj as T;
        }

        /// <summary>
        /// Serialize object into XML for WS with encoding UTF8
        /// </summary>
        /// <param name="obj">The object to be transformed</param>
        /// <param name="action">The action to be transformed</param>
        /// <param name="prefixEnvelope">The prefix envelop default is "soap"</param>
        /// <param name="xmlns">The namespace to be transformed</param>
        /// <returns>The string in XML</returns>
        public static string SerializeToWS(this object obj, string action, string prefixEnvelope = "soap", params string[] xmlns)
        {
            string xml = obj.SerializeUTF8();

            return SerializeToWSAll(xml, obj, action, prefixEnvelope, xmlns);
        }

        /// <summary>
        /// Serialize object into XML for WS with encoding iso-8859-1
        /// </summary>
        /// <param name="obj">The object to be transformed</param>
        /// <param name="action">The action to be transformed</param>
        /// <param name="prefixEnvelope">The prefix envelop default is "soap"</param>
        /// <param name="xmlns">The namespace to be transformed</param>
        /// <returns>The string in XML</returns>
        public static string SerializeToWSISO(this object obj, string action, string prefixEnvelope = "soap", params string[] xmlns)
        {
            string xml = obj.Serialize();

            return SerializeToWSAll(xml, obj, action, prefixEnvelope, xmlns);
        }

        /// <summary>
        /// Extract Result Xml WS
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="action"></param>
        /// <param name="prefixNs"></param>
        /// <param name="xmlns"></param>
        /// <returns></returns>
        public static string ExtractResultXmlWS(this string xml, string action, string prefixNs = "ns1", string xmlns = "http://tempuri.org/")
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var root = doc.DocumentElement;

                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace(prefixNs, xmlns);

                var resultNode = root.SelectSingleNode(string.Format("//{0}", action), nsmgr);

                if (!resultNode.IsNull())
                {
                    return resultNode.FirstChild.Value.IsNullOrWhiteSpace()
                        ? resultNode.FirstChild.OuterXml
                        : resultNode.FirstChild.Value;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string SerializeToWSAll(string xml, object obj, string action, string prefixEnvelope = "soap", params string[] xmlns)
        {
            var soapOpenTag = string.Format("<{0}:Envelope {1}><{0}:Body><{2}>", prefixEnvelope, string.Join(" ", xmlns), action);
            var soapCloseTag = string.Format("</{1}></{0}:Body></{0}:Envelope>", prefixEnvelope, action.Split(' ')[0]);

            var objectOpenTag = string.Format("<{0}[\\s\\S]*?>", obj.GetType().Name);
            var objectCloseTag = string.Format("</{0}>", obj.GetType().Name);

            return System.Text.RegularExpressions.Regex.Replace(xml, objectOpenTag, soapOpenTag).Replace(objectCloseTag, soapCloseTag);
        }

        #endregion

        #region "  JSON Serialization  "

        /// <summary>
        /// Transform object into json string.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="typeNameHandling">
        /// None = 0: Do not include the .NET type name when serializing types.
        /// Objects = 1: Include the .NET type name when serializing into a JSON object structure.
        /// Arrays = 2: Include the .NET type name when serializing into a JSON array structure.
        /// All = 3: Always include the .NET type name when serializing.
        /// Auto = 4: Include the .NET type name when the type of the object being serialized is not the same as its declared type.
        /// </param>
        /// <param name="ignoreNullValue">
        /// Include = 0: Include null values when serializing and deserializing objects.
        /// Ignore = 1: Ignore null values when serializing and deserializing objects.
        /// </param>
        /// <param name="useSnakeCaseNamingStrategy">Set to use snake case naming strategy</param>
        /// <returns>The string in json</returns>
        public static string ToJsonString(this object obj, string typeNameHandling = "None",
            bool ignoreNullValue = false, bool useSnakeCaseNamingStrategy = false)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new ProxyContractResolver(),
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (ignoreNullValue)
                settings.NullValueHandling = NullValueHandling.Ignore;

            if (!typeNameHandling.IsNullOrEmpty())
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>(true);

            if (useSnakeCaseNamingStrategy)
                settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Transform object into json string.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="typeNameHandling">
        /// None = 0: Do not include the .NET type name when serializing types.
        /// Objects = 1: Include the .NET type name when serializing into a JSON object structure.
        /// Arrays = 2: Include the .NET type name when serializing into a JSON array structure.
        /// All = 3: Always include the .NET type name when serializing.
        /// Auto = 4: Include the .NET type name when the type of the object being serialized is not the same as its declared type.
        /// </param>
        /// <param name="ignoreNullValue">
        /// Include = 0: Include null values when serializing and deserializing objects.
        /// Ignore = 1: Ignore null values when serializing and deserializing objects.
        /// </param>
        /// <param name="ignoreProperties">The ignore properties</param>
        /// <param name="useSnakeCaseNamingStrategy">Set to use snake case naming strategy</param>
        /// <returns>The string in json</returns>
        public static string ToJsonString(this object obj, string typeNameHandling = "None", bool ignoreNullValue = false,
            bool useSnakeCaseNamingStrategy = false, params string[] ignoreProperties)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new IgnorePropertyContractResolver(ignoreProperties),
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (ignoreNullValue)
                settings.NullValueHandling = NullValueHandling.Ignore;

            if (!typeNameHandling.IsNullOrEmpty())
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>(true);

            if (useSnakeCaseNamingStrategy)
                settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="obj"></param>
        /// <param name="typeNameHandling"></param>
        /// <param name="ignoreNullValue"></param>
        /// <param name="useSnakeCaseNamingStrategy"></param>
        /// <param name="shouldAttributePropertiesBeIgnored"></param>
        /// <returns></returns>
        public static string ToJsonString<TAttribute>(this object obj, bool shouldAttributePropertiesBeIgnored, string typeNameHandling = "None",
            bool ignoreNullValue = false, bool useSnakeCaseNamingStrategy = false) where TAttribute : Attribute
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new AttributeContractResolver<TAttribute>(shouldAttributePropertiesBeIgnored),
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (ignoreNullValue)
                settings.NullValueHandling = NullValueHandling.Ignore;

            if (!typeNameHandling.IsNullOrEmpty())
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>(true);

            if (useSnakeCaseNamingStrategy)
                settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="obj"></param>
        /// <param name="typeNameHandling"></param>
        /// <param name="ignoreNullValue"></param>
        /// <param name="useSnakeCaseNamingStrategy">Set to use snake case naming strategy</param>
        /// <returns></returns>
        public static string ToJsonString<TAttribute>(this object obj, string typeNameHandling = "None",
            bool ignoreNullValue = false, bool useSnakeCaseNamingStrategy = false) where TAttribute : Attribute
        {
            return obj.ToJsonString<TAttribute>(true, typeNameHandling, ignoreNullValue, useSnakeCaseNamingStrategy);
        }

        /// <summary>
        /// Transform json string into object .
        /// </summary>
        /// <typeparam name="T">The type to be transformed</typeparam>
        /// <param name="value">The value in json to be transform</param>
        /// <param name="typeNameHandling">The type name handling</param>
        /// <param name="useSnakeCaseNamingStrategy">Set to use snake case naming strategy</param>
        /// <returns>The object transformed</returns>
        public static T JsonTo<T>(this string value, string typeNameHandling = "None", bool useSnakeCaseNamingStrategy = false) where T : class
        {
            var settings = new JsonSerializerSettings()
            {
                Converters = new JsonConverter[] { new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() } },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (!typeNameHandling.IsNullOrEmpty())
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>(true);

            if (useSnakeCaseNamingStrategy)
                settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

            if (settings.TypeNameHandling == TypeNameHandling.Objects)
                return JsonConvert.DeserializeObject(value, settings) as T;
            else
                return JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// Transform json string into object .
        /// </summary>        
        /// <param name="value">The value in json to be transform</param>
        /// <param name="type">The type of object to be transform</param>
        /// <param name="typeNameHandling">The type name handling</param>
        /// <param name="useSnakeCaseNamingStrategy">Set to use snake case nming strategy</param>
        /// <returns>The object transformed</returns>
        public static object JsonTo(this string value, Type type, string typeNameHandling = "None", bool useSnakeCaseNamingStrategy = false)
        {
            var settings = new JsonSerializerSettings()
            {
                Converters = new JsonConverter[] { new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() } },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (!typeNameHandling.IsNullOrEmpty())
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>(true);

            if (useSnakeCaseNamingStrategy)
                settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

            if (settings.TypeNameHandling == TypeNameHandling.Objects)
                return JsonConvert.DeserializeObject(value, settings);
            else
                return JsonConvert.DeserializeObject(value, type, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T CloneTyped<T>(this T value) where T : class
        {
            var settings = new JsonSerializerSettings()
            {
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() }
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };

            var json = JsonConvert.SerializeObject(value, settings);

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Clone<T>(this T value) where T : class
        {
            var settings = new JsonSerializerSettings()
            {
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() }
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };

            var json = JsonConvert.SerializeObject(value, settings);

            return JsonConvert.DeserializeObject(json, typeof(T), settings);
        }

        /// <summary>
        /// Try parse strin to json, if error get JsonReaderException end trim in message to get stack trace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static object TryJsonTo<T>(this Exception ex) where T : class
        {
            try
            {
                return ex.Message.JsonTo<T>();
            }
            catch (JsonReaderException)
            {
                return new { result = ex.TrimMessage() };
            }
        }

        #endregion
    }

    internal class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}
