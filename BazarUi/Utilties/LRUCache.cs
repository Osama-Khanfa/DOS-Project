using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

class LRUCache<TKey, TValue>
{
    private readonly IMemoryCache _memoryCache;
    private readonly LinkedList<TKey> _accessOrder;
    private readonly object _lock = new object();

    public LRUCache()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _accessOrder = new LinkedList<TKey>();
    }

    public TValue Get(TKey key)
    {
        lock (_lock)
        {
            if (_memoryCache.TryGetValue(key, out TValue value))
            {
                // Update access order
                _accessOrder.Remove(key);
                _accessOrder.AddLast(key);
                return value;
            }

            return default; // or throw an exception, depending on your needs
        }
    }

    public void Update(TKey key, TValue valueToUpdate)
    {
        lock (_lock)
        {
            if (_memoryCache.TryGetValue(key, out TValue value))
            {
                _memoryCache.Set(key, valueToUpdate);
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        lock (_lock)
        {
            // Remove the least recently used item if the cache is full
            if (_accessOrder.Count >= 3)
            {
                var leastRecentlyUsedKey = _accessOrder.First.Value;
                _accessOrder.RemoveFirst();
                _memoryCache.Remove(leastRecentlyUsedKey);
            }

            // Add the new item
            _memoryCache.Set(
                key,
                value,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }
            );

            // Update access order
            _accessOrder.AddLast(key);
        }
    }
}
