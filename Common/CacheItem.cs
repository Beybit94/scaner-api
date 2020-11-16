using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    // <summary>Represents an item in the cache.</summary>
    public class CacheItem
    {
        readonly object _obj;
        readonly DateTime _expiresAt;

        #region .ctor

        public CacheItem(object obj, DateTime expiresAt)
        {
            _obj = obj;
            _expiresAt = expiresAt;
        }

        public CacheItem(object obj)
        {
            _obj = obj;
            _expiresAt = DateTime.MinValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The data object stored in this item
        /// </summary>
        public object Data
        {
            get { return _obj; }
        }

        /// <summary>
        /// The date/time at which this object expires
        /// </summary>
        public DateTime ExpiresAt
        {
            get { return _expiresAt; }
        }

        /// <summary>
        /// True if this object expires, false otherwise
        /// </summary>
        public bool Expires
        {
            get { return (ExpiresAt != DateTime.MinValue); }
        }

        #endregion
    }
}
