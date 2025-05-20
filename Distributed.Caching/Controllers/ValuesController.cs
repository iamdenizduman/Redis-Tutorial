using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name);
            await _distributedCache.SetAsync("lastname", Encoding.UTF8.GetBytes(surname));
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Set()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surname = Encoding.UTF8.GetString(await _distributedCache.GetAsync("lastname"));
            return Ok(name + " " + surname);
        }
    }
}
