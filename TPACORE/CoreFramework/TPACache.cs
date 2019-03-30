using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace TPA.CoreFramework
{
    public class TPACache
    {
        private const string CURRENT_STATE_KEY = "CURRENT_STATE";
        public static string LOGIN_KEY = "LOGIN_KEY";
        public static string STUDENT_LOGIN_INFO = "STUDENT_LOGIN_INFO";
        public static string STUDENT_ID_TO_EVALUATE = "STUDENT_ID_TO_EVALUATE";
        private static ObjectCache cache = MemoryCache.Default;
        public static void SetItem(string key, object value, TimeSpan tsOffset)
        {
            CacheItemPolicy cacheItemPolicy=new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.Add(tsOffset);
            cache.Set(key, value, cacheItemPolicy);
        }
        public static object GetItem(string key)
        {
            ObjectCache cache = MemoryCache.Default;
            return cache[key];
        }
        public static void ResetCache()
        {
            throw new NotImplementedException();
        }
        public static void RemoveItem(string key)
        {
            cache.Remove(key);
        }
    }

}
