using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Baibaocp.LotteryOrdering.WebApi.Controllers
{
    /// <inheritdoc/>
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ILotteryOrderingMessageService _lotteryOrderingMessageService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotteryOrderingMessageService"></param>
        public OrdersController(ILotteryOrderingMessageService lotteryOrderingMessageService)
        {
            _lotteryOrderingMessageService = lotteryOrderingMessageService;
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
        ///  { "lvpOrderId": "100009152237734312134", "lvpVenderId": "100800345", "lvpUserId": 10000, "lotteryId": 10122, "lotteryPlayId": 10061053,  "issueNumber": 18033047,  "investCode": "01,02,03,04,05",  "investType": false,  "investCount": 1,  "investTimes": 1,  "investAmount": 200 }
        /// </summary>
        /// <param name="message"></param>
        [HttpPost]
        public async void Post([FromBody] LvpOrderMessage message)
        {
            await _lotteryOrderingMessageService.PublishAsync(message);
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
