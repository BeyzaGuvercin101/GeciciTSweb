using GeciciTSweb.Application.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace GeciciTSweb.Application.Helpers
{
    public class MemoryCacheHelper : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add(string key, object value, int durationDays = 15)
        {
            _memoryCache.Set(key, value, TimeSpan.FromDays(durationDays));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
