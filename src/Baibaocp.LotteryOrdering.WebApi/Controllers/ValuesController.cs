using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Fighting.ApplicationServices.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.WebApi.Controllers
{
    /// <inheritdoc/>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IOrderingApplicationService _orderingApplicationService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderingApplicationService"></param>
        public ValuesController(IOrderingApplicationService orderingApplicationService)
        {
            _orderingApplicationService = orderingApplicationService;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        ///  GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<LotteryMerchanteOrder> Get(string id)
        {
            var order = await _orderingApplicationService.FindOrderAsync(id);
            return order;
        }

        /// <summary>
        ///  POST api/values
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        ///  PUT api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        ///  DELETE api/values/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
