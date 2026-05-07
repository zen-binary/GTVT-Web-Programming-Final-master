using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Project_Final.Services.Impl
{
    public class RedisCacheService
    {
        private readonly IServer _server;
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
            _server = ConnectionMultiplexer.Connect("localhost:6379").GetServer("localhost", 6379);
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expirationTime)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            };

            var jsonData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, jsonData, options);
        }

        public List<string> GetCacheKeys(string prefixKey)
        {
            return _server.Keys(pattern: prefixKey + "*", pageSize: 1000)
                .Select(Key => Key.ToString())
                .ToList();
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);

            if (jsonData is null)
                return default;
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task<List<T>> GetCachesAsync<T>(string prefixKey)
        {
            var results = new List<T>();
            IEnumerable<RedisKey> keys;
            keys = _server.Keys(pattern: prefixKey + "*", pageSize: 1000);
            foreach (var key in keys)
            {
                results.Add(JsonSerializer.Deserialize<T>(await _cache.GetStringAsync(key)));
            }
            return results;
        }

        public void DeleteByKey(string key)
        {
            _cache.Remove(key);
        }
    }
}
