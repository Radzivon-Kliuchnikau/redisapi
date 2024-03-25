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
            db.SetAdd("PlatformSet", serializedPlatform);
        }

        public IEnumerable<Platform>? GetAllPlatforms()
        {
            var db = redisMultiplexer.GetDatabase();

            var completeSet = db.SetMembers("PlatformSet");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val!)).ToList();

                return obj!;
            }

            return null;
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