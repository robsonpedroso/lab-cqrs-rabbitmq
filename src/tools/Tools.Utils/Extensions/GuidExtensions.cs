using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Tools.Utils.Extensions
{
    public static class GuidExtensions
    {
        /// <summary>
        /// Check guid is empty == 00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <param name="guid">The value guid</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid.IsNull() || guid == Guid.Empty; ;
        }

        /// <summary>
        /// Check guid is empty nullable == 00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <param name="guid">The value guid nullable</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsEmpty(this Guid? guid)
        {
            return guid.IsNull() || guid == Guid.Empty;
        }

        /// <summary>
        /// Transform the guid empty into DBNull data type.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>The guid nullable or empty</returns>
        public static Guid? ToDBNull(this Guid guid)
        {
            return (guid == Guid.Empty ? null : (Guid?)guid);
        }

        /// <summary>
        /// Take hyphen off from guid value
        /// </summary>
        /// <param name="value">The guid value</param>
        /// <returns>The value cleared</returns>
        public static string TakeHyphenOff(this Guid value)
        {
            return value.AsString().Replace("-", string.Empty);
        }
    }
}
