using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MSS.API.Common.DistributedEx
{
    //
    // 摘要:
    //     Extension methods for setting data in an Microsoft.Extensions.Caching.Distributed.IDistributedCache.
    public static class DistributedCacheExtensions
    {
        public static long? GetLong(this IDistributedCache cache, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return RedisHelper.Get<long?>(key);
        }

        public static void SetLong(this IDistributedCache cache, string key, long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            RedisHelper.Set(key, value);
        }

        public static bool HMSet(this IDistributedCache cache, string key, params object[] keyValues)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.HMSet(key, keyValues);
        }


        public static async Task<bool> HMSetAsync(this IDistributedCache cache, string key, params object[] keyValues)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await RedisHelper.HMSetAsync(key, keyValues);
        }

        public static T[] HMGet<T>(this IDistributedCache cache, string key, params string[] fields)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.HMGet<T>(key, fields);
        }

        public static async Task<T[]> HMGetAsync<T>(this IDistributedCache cache, string key, params string[] fields)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await RedisHelper.HMGetAsync<T>(key, fields);
        }

        public static bool HSet(this IDistributedCache cache, string key, string field, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.HSet(key, field, value);
        }
        public static async Task<bool> HSetAsync(this IDistributedCache cache, string key, string field, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await RedisHelper.HSetAsync(key, field, value);
        }

        public static async Task<bool> HSetAsync(this IDistributedCache cache, string key, dynamic obj)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            bool ret = false;
            try
            {
                JObject jobj = JObject.FromObject(obj);
                foreach (var o in jobj.Children())
                {
                    var t = o as JProperty;
                    await RedisHelper.HSetAsync(key,t.Name, t.Value);
                }
                ret = true;
            }
            catch (Exception ex) { }
            return ret;
        }

        public static T HGet<T>(this IDistributedCache cache, string key, string field)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.HGet<T>(key, field);
        }
        public static async Task<T> HGetAsync<T>(this IDistributedCache cache, string key, string field)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await RedisHelper.HGetAsync<T>(key, field);
        }

        public static T Get<T>(this IDistributedCache cache, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.Get<T>(key);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await RedisHelper.GetAsync<T>(key);
        }

        public static async Task SetAsync(this IDistributedCache cache, string key, object value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (options != null)
            {
                var offsettime = options.AbsoluteExpirationRelativeToNow;
                int expireSecond = (int)offsettime.Value.TotalSeconds;
                await RedisHelper.SetAsync(key, value, expireSecond);
            }
            else
            {
                await RedisHelper.SetAsync(key, value);
            }
        }

    }
}