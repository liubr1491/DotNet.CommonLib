using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.Cache
{
    public static class CacheHelper
    {
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">缓存名称</param>
        /// <param name="timeOut">缓存时长</param>
        /// <param name="func">无缓存情况下，读取缓存方法</param>
        /// <param name="ct">缓存类型</param>
        /// <returns>缓存对象</returns>
        public static T GetCache<T>(string key, int timeOut, Func<T> func, CacheType ct = CacheType.Minute)
        {
            var temp = GetCache(key);
            T cachedObject;
            if (temp == null)
            {
                cachedObject = func();
                if (cachedObject != null)
                {
                    Add(key, cachedObject, timeOut, ct);
                }
            }
            else
            {
                cachedObject = (T)temp;
            }
            return cachedObject;
        }

        private static void Add(string key, object cachedObject, int timeOut, CacheType ct)
        {
            throw new NotImplementedException();
        }

        private static object GetCache(string key)
        {
            throw new NotImplementedException();
        }
    }
}
