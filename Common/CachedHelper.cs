using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
   public class CachedHelper
    {
        static RedisHelper redis = new RedisHelper(1);
        #region 存储缓存string
        /// <summary>
        /// 存储缓存string
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static bool SetCached(string Key, string  Value, DateTime? expiry = null)
        {
            TimeSpan? ts; //过期时间
            if (expiry == null)
            {
                ts = null;
            }
            else
            {
                ts = expiry - DateTime.Now;
            }
            return redis.StringSet(Key, Value, ts);
        }
        #endregion
        #region 存储缓存obj
        /// <summary>
        /// 存储缓存obj
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static bool SetCached<T>(string Key, T Value, DateTime? expiry = null)
        {
            TimeSpan? ts; //过期时间
            if (expiry == null)
            {
                ts = null;
            }
            else
            {
                ts = expiry - DateTime.Now;
            }
            return redis.StringSet<T>(Key, Value, ts);
        }
        #endregion
        #region 获取缓存
        /// <summary>
        /// 获取缓存string
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetCached(string Key)
        {
            var value = redis.StringGet(Key);
            return value;
        }
       
        #endregion
        #region 获取缓存obj
        /// <summary>
        /// 获取缓存obj
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T GetCached<T>(string Key) where T:class
        {
            var value = redis.StringGet<T>(Key);
            return value;
        }
        #endregion
        #region 清除缓存key
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool KeyDelete(string Key)
        {
            var value = redis.KeyDelete(Key);
            return value;
        }
        #endregion
        #region cookie存储缓存
        /// <summary>
        /// cookie存储缓存
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string Key, string Value, DateTime? expires=null)
        {
            //创建一个cookie
            HttpCookie cookie = new HttpCookie(Key);
            cookie.Value = Value;
            if (expires != null)
            {
                cookie.Expires = (DateTime)expires;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);//分布式缓存session共享

        }
        #endregion
        #region cookie获取
        /// <summary>
        /// cookie获取
        /// </summary>
        /// <param name="Key">键</param>
        /// <returns></returns>
        public static string GetCookie(string Key)
        {
            if (HttpContext.Current.Request.Cookies[Key] != null)
            {
                return HttpContext.Current.Request.Cookies[Key].Value;
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region cookid删除
        public static bool DelCookie(string Key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            return true;
        }
        #endregion
    }
}
