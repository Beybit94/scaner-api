using Business.Models.Dictionary;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
    public static class CacheDictionaryManager
    {
        public static T[] GetDictionary<T>() where T : BaseDictionary
        {
            T[] result = CacheManager.Get(typeof(T).FullName, DictionaryManager.GetDictionary<T>);
            return result;
        }

        public static T[] GetDictionaryShort<T>() where T : BaseDictionaryShort
        {
            T[] result = CacheManager.Get(typeof(T).FullName + "Short", DictionaryManager.GetDictionaryShort<T>);
            return result;
        }
    }
}
