using Project_Final.Models;
using Project_Final.Models.Response;
using Project_Final.Utils;

namespace Project_Final.Services.Impl
{
    public class CardOrderService : ICardOrderService
    {
        private readonly RedisCacheService _redisCacheService;
        public CardOrderService(RedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public Response<CardOrder> Delete(string id)
        {
            _redisCacheService.DeleteByKey(Constant.CARD_ORDER_CACHE_KEY_PREFIX + id);
            return Response<CardOrder>.OfSuccessed();
        }

        public Response<CardOrder> GetCardOrder(string id)
        {
            return Response<CardOrder>.OfSuccessed(
                _redisCacheService.GetCacheAsync<CardOrder>(Constant.CARD_ORDER_CACHE_KEY_PREFIX + id).Result);
        }
        public Response<List<CardOrder>> GetCardOrders()
        {
            var data = _redisCacheService.GetCachesAsync<CardOrder>(Constant.CARD_ORDER_CACHE_KEY_PREFIX).Result.OrderBy(c => c.CreatedAt).ToList();
            return Response<List<CardOrder>>.OfSuccessed(data,1 , data.Count, data.Count);
        }

        public Response<List<string>> GetIds()
        {
            var data = _redisCacheService.GetCacheKeys(Constant.CARD_ORDER_CACHE_KEY_PREFIX)
                .Select(v => v.Substring(Constant.CARD_ORDER_CACHE_KEY_PREFIX.Length))
                .ToList();
            Console.WriteLine("GetIds: " + data);
            return Response<List<string>>.OfSuccessed(data, 1, data.Count, data.Count);
        }

        public Response<string> Upsert(CardOrder cardOrder)
        {
            if (cardOrder.Id == null)
            {
                cardOrder.Id = Guid.NewGuid().ToString();
            }
            cardOrder.CreatedAt = DateTime.Now;
            var key = Constant.CARD_ORDER_CACHE_KEY_PREFIX + cardOrder.Id;
            _redisCacheService.SetCacheAsync(key, cardOrder, TimeSpan.FromMinutes(3 * 24 * 60)).Wait(10);
            return Response<string>.OfSuccessed(cardOrder.Id);
        }
    }
}
