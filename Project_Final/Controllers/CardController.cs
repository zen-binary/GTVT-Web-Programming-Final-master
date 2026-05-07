using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Final.Services.Impl;
using Project_Final.Utils;

namespace Project_Final.Controllers
{
    //[Authorize]
    public class CardController : Controller
    {
        private readonly RedisCacheService _redisCacheService;

        public CardController(RedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public IActionResult Index()
        {
            var data = _redisCacheService.GetCacheKeys(Constant.CARD_ORDER_CACHE_KEY_PREFIX)
                .Select(v => v.Substring(Constant.CARD_ORDER_CACHE_KEY_PREFIX.Length))
                .ToList();
            return View(data);
        }
    }
}
