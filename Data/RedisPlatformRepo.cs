using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo(IConnectionMultiplexer redisMultiplexer) : IPlatformRepo
    {
        public Platform? ChangePlatformById(string platformId, Platform platform)
        {
            var db = redisMultiplexer.GetDatabase();

            var platformObj = db.HashGet("hashplatform", platformId);

            if (!string.IsNullOrEmpty(platformObj))
            {
                var serializedPlatform = JsonSerializer.Serialize(platform);

                db.HashSet("hashplatform", [new HashEntry(platformId, serializedPlatform)]);

                var createdPlatform = db.HashGet("hashplatform", platformId);

                return JsonSerializer.Deserialize<Platform>(createdPlatform!);
            }

            return null;

        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }

            var db = redisMultiplexer.GetDatabase();

            var serializedPlatform = JsonSerializer.Serialize(platform);

            // db.StringSet(platform.Id, serializedPlatform);
            // db.SetAdd("PlatformSet", serializedPlatform);

            db.HashSet("hashplatform", [new HashEntry(platform.Id, serializedPlatform)]);
        }

        public IEnumerable<Platform>? GetAllPlatforms()
        {
            var db = redisMultiplexer.GetDatabase();

            // var completeSet = db.SetMembers("PlatformSet");

            var completeHash = db.HashGetAll("hashplatform");

            if (completeHash.Length > 0)
            {
                var obj = Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value!)).ToList();

                return obj!;
            }

            return null;
        }

        public Platform? GetPlatformById(string platformId)
        {
            var db = redisMultiplexer.GetDatabase();

            // var platform = db.StringGet(platformId);

            var platform = db.HashGet("hashplatform", platformId);

            if (!string.IsNullOrEmpty(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform!);
            }

            return null;
        }

        public bool RemovePlatformById(string platformId)
        {
            var db = redisMultiplexer.GetDatabase();

            if (db.HashGet("hashplatform", platformId).HasValue)
            {
                db.HashDelete("hashplatform", platformId);

                return true;
            }

            return false;
        }
    }
}