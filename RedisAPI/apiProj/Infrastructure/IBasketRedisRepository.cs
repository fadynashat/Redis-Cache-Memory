using apiProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProj.Infrastructure
{
    public interface IBasketRedisRepository : IRedisRepository<CustomerBasket>
    {

    }
}
