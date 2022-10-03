namespace Fintech.ExternalService.CloudService.Caching;

public interface ICacheService
{
    object GetData(string Key);
    T GetData<T>(string Key) where T : class, new();
    bool IsAdd(string Key);
    Task<T> GetDataAsync<T>(string Key) where T : class, new();
    Task<bool> SetDataAsync(string key, object data, double duration);
    bool SetData<T>(string key, T data) where T : class, new();
    Task<bool> SetDataAsync<T>(string key, T data) where T : class, new();
    bool SetDataForSearch<T>(List<T> data, double ExpireMinute) where T : class, new();
    bool SetDataWithExpire<T>(string key, double ExpireMinute, T data) where T : class, new();
    Task<bool> SetDataWithExpireAsync<T>(string key, double ExpireMinute, T data) where T : class, new();
    Task<bool> DeleteKeyAsync(string key);
    Task<bool> ExistKeyAsync(string key);
    bool DeleteKey(string key);
    Task<List<T>> Search<T>(T Model) where T : class, new();
}
