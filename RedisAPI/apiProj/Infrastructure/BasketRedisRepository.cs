using apiProj.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProj.Infrastructure
{
    public class BasketRedisRepository : RedisRepository<CustomerBasket>, IBasketRedisRepository
    {
        private readonly ConnectionMultiplexer _redis;
        public BasketRedisRepository(ConnectionMultiplexer redis) :base (redis)
        {
            _redis = redis;
        }

    }
}
