using apiProj.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace apiProj.Infrastructure
{
    public interface IRedisRepository<Entity> where Entity : class
    {
        IEnumerable<string> GetUsers();



        Task<bool> SaveItemToCache(string itemKey, Entity entity);
        Task<bool> SaveItemToCache(string itemKey, Entity entity, TimeSpan absoluteExpiration);
        Task<Entity> GetItemFromCache(string itemKey);
        Task<bool> UpdateItemFromCache(string itemKey, Entity entity);
        Task<bool> DeleteItemFromCache(string itemKey);
    }
}
