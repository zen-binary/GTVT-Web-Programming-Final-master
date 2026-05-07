using Microsoft.AspNetCore.Mvc;
using Project_Final.Models;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Services;
using System.Security.Claims;

namespace Project_Final.Api
{
    [ApiController]
    [Route("/admin/api/bill")]
    public class BillApi : ControllerBase
    {
        private readonly ISellService sellService;

        public BillApi(ISellService sellService)
        {
            this.sellService = sellService;
        }

        [HttpPost("create/{id}")]
        public Response<OrderResponse> Create(string id)
        {
            return sellService.Create(id);
        }

        [HttpGet("{id}")]
        public Response<OrderResponse> Get(long id)
        {
            return sellService.Get(id);
        }

        [HttpGet]
        public Response<List<OrderResponse>> Gets([FromQuery] string? createdBy,
            [FromQuery] OrderStatus status = OrderStatus.ALL,
            [FromQuery] DateTime from = default,
            [FromQuery] DateTime to = default,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            return sellService.Gets(createdBy, status, from, to, page, size);
        }

        [HttpGet("details/{orderId}")]
        public Response<List<OrderDetailResponse>> GetDetails(long orderId)
        {
            return sellService.GetDetails(orderId);
        }
    }
}
