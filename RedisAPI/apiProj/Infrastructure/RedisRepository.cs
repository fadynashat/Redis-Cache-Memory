using apiProj.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProj.Infrastructure
{
    public class RedisRepository<Enitity> : IRedisRepository<Enitity> where Enitity : class
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        public RedisRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }



        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }



        public async Task<bool> SaveItemToCache(string itemKey, Enitity entity)
        {
            var created = await _database.StringSetAsync(itemKey, JsonConvert.SerializeObject(entity));
            return created;
        }

        public async Task<bool> SaveItemToCache(string itemKey, Enitity entity, TimeSpan absoluteExpiration)
        {
            var created = await _database.StringSetAsync(itemKey, JsonConvert.SerializeObject(entity), absoluteExpiration);
            return created;
        }

        public async Task<Enitity> GetItemFromCache(string itemKey)
        {
            var entity = await _database.StringGetAsync(itemKey);

            if (entity.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Enitity>(entity);
        }

        public async Task<bool> UpdateItemFromCache(string itemKey, Enitity entity)
        {
            var obj = await _database.StringGetAsync(itemKey);

            if (obj.IsNullOrEmpty)
            {
                return false;
            }

            var updated = await _database.StringSetAsync(itemKey, JsonConvert.SerializeObject(entity));
            return updated;
        }

        public async Task<bool> DeleteItemFromCache(string itemKey)
        {
            return await _database.KeyDeleteAsync(itemKey);
        }
        
        
        
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
