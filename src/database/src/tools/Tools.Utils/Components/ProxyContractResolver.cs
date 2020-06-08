using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace Tools.Utils.Components
{
    public class ProxyContractResolver : DefaultContractResolver
    {
        protected virtual bool IsProxy(Type type)
        {
            var proxyClasses = new string[]
            {
                "NHibernate.Proxy.DynamicProxy.IProxy",
                "NHibernate.Proxy.INHibernateProxy"
            };

            return type.GetInterfaces()
                .Any(i => proxyClasses.Contains(i.FullName));
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (IsProxy(objectType))
                return base.CreateContract(objectType.BaseType);
            else
                return base.CreateContract(objectType);
        }
    }
}
