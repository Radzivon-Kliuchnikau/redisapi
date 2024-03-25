using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo(IConnectionMultiplexer redisMultiplexer) : IPlatformRepo
    {
        public void ChangePlatformById(string platformId)
        {
            throw new NotImplementedException();
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }

            var db = redisMultiplexer.GetDatabase();

            var serializedPlatform = JsonSerializer.Serialize(platform);

            db.StringSet(platform.Id, serializedPlatform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Platform? GetPlatformById(string platformId)
        {
            var db = redisMultiplexer.GetDatabase();

            var platform = db.StringGet(platformId);

            if (!string.IsNullOrEmpty(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform!);
            }

            return null;
        }

        public void RemovePlatformById(string platformId)
        {
            throw new NotImplementedException();
        }
    }
}