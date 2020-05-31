using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace CQRS.Tools.Utils.Components
{
    public class AttributeContractResolver<TAttribute> : ProxyContractResolver where TAttribute : Attribute
    {
        private readonly bool ignore;

        public AttributeContractResolver(bool ignore = true)
            => this.ignore = ignore;

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var hasAttribute = member.CustomAttributes.Any(p => p.AttributeType == typeof(TAttribute));

            if (hasAttribute)
                return ignore ? null : base.CreateProperty(member, memberSerialization);

            return ignore ? base.CreateProperty(member, memberSerialization) : null;
        }
    }
}
