using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class CacheManager
    {
        #region Static

        public static object CacheLocker = new object();
        private static readonly Dictionary<string, CacheItem> CachedData = new Dictionary<string, CacheItem>();

        #endregion

        #region Methods

        public static void AddItem<T>(string key, T obj)
        {
            lock (CacheLocker)
                CachedData.Add(key, new CacheItem(obj));
        }

        public static T Get<T>(string key)
        {
            lock (CacheLocker)
            {
                return CachedData.ContainsKey(key) ? (T)CachedData[key].Data : default(T);
            }
        }

        public static T Get<T>(string key, Func<T> cacheFunc)
        {
            lock (CacheLocker)
            {
                if (cacheFunc == null)
                    throw new ArgumentNullException("cacheFunc");

                if (CachedData.ContainsKey(key))
                    return (T)CachedData[key].Data;

                T cacheThis = cacheFunc();
                CachedData.Add(key, new CacheItem(cacheThis));

                return cacheThis;
            }
        }

        #endregion
    }
}
