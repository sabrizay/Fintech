using Fintech.ExternalService.CloudService.Caching.Attributes;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.ExternalService.CloudService.Caching.Redis
{
    public class RedisService : ICacheService
    {
        private static string _RedisConnectionString = null;

        private readonly Lazy<ConnectionMultiplexer> lazyConnection;

        public RedisService(IConfiguration config, string RedisConnectionString = null)
        {
            _RedisConnectionString = RedisConnectionString ?? config.GetSection("RedisConnection").Value;

            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(_RedisConnectionString);
            });
        }


        private ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        #region SYNC

        public object GetData(string Key)
        {
            try
            {
                IDatabase cache = Connection.GetDatabase();

                if (cache.IsConnected(Key))
                {
                    if (cache.StringGet(Key).HasValue)
                    {

                        byte[] bytes = cache.StringGet(Key);
                        var RedisValue = (Encoding.UTF8.GetString(bytes));
                        return RedisValue;
                    }
                }
            }
            catch { }
            return default;
        }

        public T GetData<T>(string Key) where T : class, new()
        {
            try
            {

                IDatabase cache = Connection.GetDatabase();

                if (cache.IsConnected(Key))
                {
                    if (cache.StringGet(Key).HasValue)
                    {

                        byte[] bytes = cache.StringGet(Key);
                        var RedisValue = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                        return RedisValue;
                    }
                }
            }
            catch { }
            return default;
        }
        public T Query<T>(string Key) where T : class, new()
        {
            try
            {

                IDatabase cache = Connection.GetDatabase();

                if (cache.IsConnected(Key))
                {
                    if (cache.StringGet(Key).HasValue)
                    {

                        byte[] bytes = cache.StringGet(Key);
                        var RedisValue = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                        return RedisValue;
                    }
                }
            }
            catch { }
            return default;
        }
        public bool SetData<T>(string key, T data) where T : class, new()
        {

            var result = false;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, formatting: Formatting.None));
                IDatabase cache = Connection.GetDatabase();


                result = cache.StringSet(key, bytes);

            }
            catch { }
            return result;
        }
        public async Task<List<T>> Search<T>(T Model) where T : class, new()
        {

            List<T> list = new();
            try
            {

                IDatabase cache = Connection.GetDatabase();
                string Key = "";


                Key = GetKey<T>(Model);


                //  return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(cache.StringGet(bytes)));
                var server = Connection.GetServer("localhost");
                var Keys = server.Keys(pattern: "*" + Key + "*");
                foreach (var key in Keys)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(key);
                    RedisValue redisValue = cache.StringGet(bytes);
                    if (redisValue.HasValue)
                    {
                        var rsl = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString((byte[])redisValue.Box()));
                        if (!list.Contains(rsl)) list.Add(rsl);
                    }
                }




            }
            catch { }
            return list;
        }

        public bool SetDataWithExpire<T>(string key, double ExpireMinute, T data) where T : class, new()
        {

            var result = false;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, formatting: Formatting.None));
                IDatabase cache = Connection.GetDatabase();

                result = cache.StringSet(key, bytes, expiry: TimeSpan.FromMinutes(ExpireMinute));

            }
            catch { }

            return result;
        }
        public bool DeleteKey(string key)
        {
            bool result = false;
            try
            {
                IDatabase cache = Connection.GetDatabase();

                result = cache.KeyDelete(key);
            }
            catch { }
            return result;
        }
        private static string GetKey<T>(T Model)
        {
            //typeof(T).GetProperties()[1].CustomAttributes

            List<string> list = new();
            try
            {
                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                {
                    if (prop.CustomAttributes.Any(a => a.AttributeType == typeof(RedisIsSearchable)))
                    {
                        var attr = prop.CustomAttributes.Where(a => a.AttributeType == typeof(RedisIsSearchable)).FirstOrDefault();
                        if ((bool)attr.ConstructorArguments.FirstOrDefault().Value)
                        {
                            if (prop.GetValue(Model) != null) list.Add(string.Format("({0}.{1}:{2})", Model.GetType().Name, prop.Name, prop.GetValue(Model)?.ToString()));
                        }
                    }
                }
            }
            catch { }

            return (list.Count > 0) ? string.Join(";", list) : typeof(T).Name;
        }

        #endregion

        #region ASYNC
        public bool SetDataForSearch<T>(List<T> data, double ExpireMinute) where T : class, new()
        {

            var result = false;
            try
            {
                IDatabase cache = Connection.GetDatabase();
                string Key = "";
                foreach (var item in data)
                {

                    Key = GetKey<T>(item);
                    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item, formatting: Formatting.None));
                    byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
                    if (cache.StringGet(keyBytes).HasValue) DeleteKey(Key);
                    cache.StringSet(keyBytes, bytes, expiry: TimeSpan.FromMinutes(ExpireMinute));
                }
                return true;
            }
            catch { }
            return result;
        }
        public async Task<T> GetDataAsync<T>(string Key) where T : class, new()
        {
            try
            {

                IDatabase cache = Connection.GetDatabase();

                if (cache.IsConnected(Key))
                {
                    if (cache.StringGet(Key).HasValue)
                    {

                        byte[] bytes = cache.StringGet(Key);
                        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));

                    }
                }
            }
            catch { }
            return default;
        }

        public async Task<bool> SetDataAsync<T>(string key, T data) where T : class, new()
        {
            var result = false;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, formatting: Formatting.None));
                IDatabase cache = Connection.GetDatabase();
                return await cache.StringSetAsync(key, bytes);
            }
            catch { }
            return result;
        }
        public async Task<bool> SetDataWithExpireAsync<T>(string key, double ExpireMinute, T data) where T : class, new()
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, formatting: Formatting.None));
                IDatabase cache = Connection.GetDatabase();
                return await cache.StringSetAsync(key, bytes, expiry: TimeSpan.FromMinutes(ExpireMinute));
            }
            catch { }
            return false;
        }
        public async Task<bool> DeleteKeyAsync(string key)
        {
            try
            {
                IDatabase cache = Connection.GetDatabase();
                bool KeyExist = await cache.KeyExistsAsync(key);
                if (KeyExist) return await cache.KeyDeleteAsync(key); else return true;


            }
            catch { }
            return false;
        }
        public async Task<bool> ExistKeyAsync(string key)
        {
            try
            {
                IDatabase cache = Connection.GetDatabase();
                return await cache.KeyExistsAsync(key);
            }
            catch { }
            return false;
        }

        public bool IsAdd(string Key)
        {
            try
            {
                IDatabase cache = Connection.GetDatabase();
                if (cache.IsConnected(Key))
                {
                    if (cache.StringGet(Key).HasValue)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        public async Task<bool> SetDataAsync(string key, object data, double duration)
        {
            var result = false;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, formatting: Formatting.None));
                IDatabase cache = Connection.GetDatabase();
                return await cache.StringSetAsync(key, bytes, expiry: TimeSpan.FromMinutes(duration));
            }
            catch { }
            return result;
        }


        #endregion
    }
}
