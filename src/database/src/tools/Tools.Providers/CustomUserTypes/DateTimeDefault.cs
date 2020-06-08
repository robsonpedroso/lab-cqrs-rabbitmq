using Gestor.Tools.Utils.Extensions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;
using System.Data.Common;

namespace Gestor.Tools.Providers.CustomUserTypes
{
    [Serializable]
    public class DateTimeDefault : IUserType
    {
        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            var xdocX = x;
            var xdocY = y;

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

            return value.AsDateTime() != DateTime.MinValue ? value : DBNull.Value;
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

            return str.AsDateTime() != DateTime.MinValue ? str : null;
        }

        public object Disassemble(object value)
        {
            if (value == null)
                return null;

            return value.AsDateTime() != DateTime.MinValue ? value : null;
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            if (names.Length != 1)
                throw new InvalidOperationException("Only expecting one column...");

            var val = rs[names[0]].AsDateTime();

            if (val != DateTime.MinValue)
                return val;
            else
                return null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = (DbParameter)cmd.Parameters[index];

            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = value.AsDateTime() != DateTime.MinValue ? value : DBNull.Value;
            }
        }

        public SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.DateTime.SqlType }; }
        }

        public Type ReturnedType
        {
            get { return typeof(DateTime); }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }
}
