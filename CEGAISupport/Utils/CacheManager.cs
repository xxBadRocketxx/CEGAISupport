using System;
using System.Collections.Generic;
using System.Runtime.Caching; // Cần tham chiếu đến System.Runtime.Caching

namespace CEGAISupport.Utils
{
    public static class CacheManager
    {
        private static readonly MemoryCache _cache = new MemoryCache("CEGAISupportCache");
        private static readonly CacheItemPolicy _defaultPolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) // Thời gian cache mặc định (ví dụ: 10 phút)
        };

        public static void AddToCache(string key, object value, CacheItemPolicy policy = null)
        {
            _cache.Set(key, value, policy ?? _defaultPolicy);
        }

        public static T GetFromCache<T>(string key) where T : class
        {
            return _cache.Get(key) as T;
        }
        //Có thể xoá cache
        public static void RemoveFromCache(string key)
        {
            if (_cache.Contains(key))
            {
                _cache.Remove(key);
            }
        }
        //Xoá tất cả
        public static void ClearCache()
        {
            // Tạo một list các key để xóa (vì không thể thay đổi collection trong khi iterate)
            List<string> keysToRemove = new List<string>();
            foreach (var item in _cache)
            {
                keysToRemove.Add(item.Key);
            }

            // Xóa từng key
            foreach (string key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }
    }
}