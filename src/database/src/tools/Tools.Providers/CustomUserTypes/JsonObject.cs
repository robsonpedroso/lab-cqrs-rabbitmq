using Gestor.Tools.Utils.Extensions;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;
using System.Data.Common;

namespace Gestor.Tools.Providers.CustomUserTypes
{
    [Serializable]
    public class JsonObject<T> : IUserType where T : class
    {
        private readonly static string typeHandling = "Objects";

        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            var xdocX = x.ToJsonString(typeHandling, true);
            var xdocY = y.ToJsonString(typeHandling, true);

            return xdocY == xdocX;
        }

        public int GetHashCode(object x)
        {
            if (x == null)
                return 0;

            return x.GetHashCode();
        }

        public object DeepCopy(object value)
        {
            if (value == null)
                return null;

            //Serialized and Deserialized using json.net so that I don't
            //have to mark the class as serializable. Most likely slower
            //but only done for convenience. 

            var serialized = value.ToJsonString(typeHandling, true);

            return serialized.JsonTo<T>(typeHandling);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            var str = cached as string;

            if (string.IsNullOrWhiteSpace(str))
                return null;

            return str.JsonTo<T>(typeHandling);
        }

        public object Disassemble(object value)
        {
            if (value == null)
                return null;

            return value.ToJsonString(typeHandling, true);
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            throw new NotImplementedException();
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            throw new NotImplementedException();
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[] { new StringClobSqlType() };
            }
        }

        public Type ReturnedType
        {
            get { return typeof(T); }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }
}
