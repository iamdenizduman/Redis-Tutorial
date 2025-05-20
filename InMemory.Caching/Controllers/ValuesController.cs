using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet(nameof(GetCache))]
        public string GetCache()
        {
            _memoryCache.Set("name", "deniz");
            if (_memoryCache.TryGetValue("name", out string value))
            {
                return value;
            }
            return "başarısız";
        }

        [HttpGet(nameof(SetKeyCachingTime))]
        public void SetKeyCachingTime()
        {
            // date 30 saniyede silinir 
            // 5 saniye içinde kullanılmaz ise yine veri silinir
            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet(nameof(GetKeyCachingTime))]
        public DateTime GetKeyCachingTime()
        {
            return _memoryCache.Get<DateTime>("date");
        }
    }
}
