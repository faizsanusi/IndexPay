using indexPay.Utilities.IUtilities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace indexPay.Utilities
{
    public class MemCache : IMemCache
    {
        private readonly IMemoryCache _memCache;

        public MemCache(IMemoryCache memoryCache)
        {
            _memCache = memoryCache;
        }
        public object Get(string key)
        {
           return _memCache.Get(key);
        }


        public void Set(string key, string value, int expiryInSeconds)
        {
            _memCache.Set(key, value, DateTimeOffset.Now.AddSeconds(expiryInSeconds));
        }
    }
}
