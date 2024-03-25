using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform platform);

        Platform? GetPlatformById(string platformId);

        IEnumerable<Platform> GetAllPlatforms();

        void RemovePlatformById(string platformId);

        void ChangePlatformById(string platformId); // TODO: Watch CRUD operations api for REDIS
    }
}