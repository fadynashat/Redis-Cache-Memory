using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using apiProj.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using apiProj.Infrastructure;

namespace apiProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRedisRepository _repository;

        public BasketController(IBasketRedisRepository repository)
        {
            _repository = repository;
        }



        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> AddBasketAsync([FromBody] CustomerBasket value)
        {
            return Ok(await _repository.SaveItemToCache(value.BuyerId, value));
        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
        {
            var basket = await _repository.GetItemFromCache(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        
        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody]CustomerBasket value)
        {
            return Ok(await _repository.UpdateItemFromCache(value.BuyerId, value));
        }
        

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasketByIdAsync(string id)
        {
            return Ok( await _repository.DeleteItemFromCache(id));
        }



    }
}
