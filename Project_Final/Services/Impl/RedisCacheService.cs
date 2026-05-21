using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Project_Final.Services.Impl
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IServer? _server;
        private static readonly ConcurrentDictionary<string, byte> _keyIndex = new();

        public RedisCacheService(IDistributedCache cache, IConfiguration configuration, ILogger<RedisCacheService> logger)
        {
            _cache = cache;

            if (!configuration.GetValue<bool>("Redis:Enabled"))
                return;

            var connectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
            try
            {
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = false;
                options.ConnectTimeout = 3000;
                var multiplexer = ConnectionMultiplexer.Connect(options);
                if (multiplexer.IsConnected)
                {
                    _server = multiplexer.GetServer(multiplexer.GetEndPoints().First());
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Redis is not available. Card cache will use in-memory storage.");
            }
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expirationTime)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            };

            var jsonData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, jsonData, options);
            _keyIndex.TryAdd(key, 0);
        }

        public List<string> GetCacheKeys(string prefixKey)
        {
            if (_server?.IsConnected == true)
            {
                try
                {
                    return _server.Keys(pattern: prefixKey + "*", pageSize: 1000)
                        .Select(key => key.ToString())
                        .ToList();
                }
                catch
                {
                    // fall through to in-memory index
                }
            }

            return _keyIndex.Keys
                .Where(key => key.StartsWith(prefixKey, StringComparison.Ordinal))
                .ToList();
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);

            if (jsonData is null)
                return default;

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task<List<T>> GetCachesAsync<T>(string prefixKey)
        {
            var results = new List<T>();
            foreach (var key in GetCacheKeys(prefixKey))
            {
                var item = await GetCacheAsync<T>(key);
                if (item is not null)
                    results.Add(item);
            }
            return results;
        }

        public void DeleteByKey(string key)
        {
            _cache.Remove(key);
            _keyIndex.TryRemove(key, out _);
        }
    }
}
