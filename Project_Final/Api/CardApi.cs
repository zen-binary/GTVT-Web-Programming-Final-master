using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Final.Models;
using Project_Final.Models.Response;
using Project_Final.Services;

namespace Project_Final.Api
{

    //[Authorize]
    [ApiController]
    [Route("/admin/api/card")]
    public class CardApi : ControllerBase
    {
        private readonly ICardOrderService _cardOrderService;
        public CardApi(ICardOrderService cardOrderService)
        {
            _cardOrderService = cardOrderService;
        }

        [HttpPost("create")]
        public Response<string> Create(CardOrder cardOrder)
        {
            cardOrder.Id = null;
            return _cardOrderService.Upsert(cardOrder);
        }

        [HttpPut("{id}")]
        public Response<string> Update(string id, CardOrder cardOrder)
        {
            cardOrder.Id = id;
            return _cardOrderService.Upsert(cardOrder);
        }

        [HttpGet("get-card-ids")]
        public Response<List<string>> GetCardOrderIds()
        {
            return _cardOrderService.GetIds();
        }

        [HttpGet("{id}")]
        public Response<CardOrder> GetCardOrder(string id)
        {
            return _cardOrderService.GetCardOrder(id);
        }

        [HttpGet]
        public Response<List<CardOrder>> GetCardOrders()
        {
            return _cardOrderService.GetCardOrders();
        }

        [HttpDelete("{id}")]
        public Response<CardOrder> Delete(string id)
        {
            return _cardOrderService.Delete(id);
        }
    }
}
