using System;
using Microsoft.Extensions.Caching.Memory;
using AprioritWebCalendar.Business.Interfaces;

namespace AprioritWebCalendar.Business.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetItem<TItem>(string key, TItem item, int timeToStoreMinutes)
        {
            _cache.Set(key, item, new TimeSpan(0, timeToStoreMinutes, 0));
        }

        public TItem GetItem<TItem>(string key)
        {
            return _cache.Get<TItem>(key);
        }

        public void RemoveItem(string key)
        {
            _cache.Remove(key);
        }
    }
}
