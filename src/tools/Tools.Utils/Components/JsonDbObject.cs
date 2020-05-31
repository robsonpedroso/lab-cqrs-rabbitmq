using CQRS.Tools.Utils.Extensions;
using Newtonsoft.Json;
using System.Linq;

namespace CQRS.Tools.Utils.Components
{
    public abstract class JsonDbObject
    {
        private bool _isLoaded;

        private string _object;

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual string Object
        {
            get
            {
                return this.ToJsonString<JsonDbIgnore>("Objects", true);
            }
            set
            {
                if (!_isLoaded || !_object.Equals(value))
                {
                    _object = value;

                    var instance = _object.JsonTo(this.GetType(), "Objects");

                    instance.GetType().GetProperties().ForEach(prop =>
                    {
                        if (!prop.CustomAttributes.Any(p => p.AttributeType == typeof(JsonIgnoreAttribute) || p.AttributeType == typeof(JsonDbIgnore)))
                        {
                            var val = prop.GetValue(instance);

                            if (!val.IsNull())
                                prop.SetValue(this, val);
                        }
                    });

                    _isLoaded = true;
                }
            }
        }
    }
}
