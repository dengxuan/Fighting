using Baibaocp.LotteryOrdering.ApplicationServices;
using Baibaocp.LotteryOrdering.Core.Entities;
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
        private readonly IApplicationServiceCluster _serviceCluster;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCluster"></param>
        public ValuesController(IApplicationServiceCluster serviceCluster)
        {
            _serviceCluster = serviceCluster;
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
        public async Task<LotteryVenderOrderEntity> Get(string id)
        {
            var applicationService = _serviceCluster.GetApplicationService<IOrderingApplicationService>();
            var order = await applicationService.FindOrderAsync(id);
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
