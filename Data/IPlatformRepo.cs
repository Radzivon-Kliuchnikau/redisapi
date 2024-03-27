using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform platform);

        Platform? GetPlatformById(string platformId);

        IEnumerable<Platform>? GetAllPlatforms();

        bool RemovePlatformById(string platformId);

        Platform? ChangePlatformById(string platformId, Platform platform); // TODO: Watch CRUD operations api for REDIS
    }
}