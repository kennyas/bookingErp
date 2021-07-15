using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Shared.Extensions
{
    public static class CacheExtenstions
    {
        public static async Task<byte[]> SetRedisCacheAsync(this IDistributedCache cache,string key, object item)
        {
            var serializedsettingModel = JsonConvert.SerializeObject(item);
            var cachedSetting = Encoding.UTF8.GetBytes(serializedsettingModel);
            await cache.SetAsync(key, cachedSetting, new DistributedCacheEntryOptions { });

            return cachedSetting;
        }

        public static async Task<T> GetRedisCachedObjectAsync<T>(this IDistributedCache cache, byte[] body)
        {
            var byteSetting = Encoding.UTF8.GetString(body);
            var deserializedSetting = JsonConvert.DeserializeObject<T>(byteSetting);

            return await Task.FromResult(deserializedSetting);
        }
    }
}
