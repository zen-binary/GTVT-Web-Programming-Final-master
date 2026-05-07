using Project_Final.Models;
using Project_Final.Models.Request;
using Project_Final.Models.Response;

namespace Project_Final.Services
{
    public interface ISellService
    {
        Response<OrderResponse> Create(string id);
        Response<OrderResponse> Get(long id);
        Response<List<OrderDetailResponse>> GetDetails(long orderId);
        Response<List<OrderResponse>> Gets(string createdBy, OrderStatus status, DateTime from, DateTime to, int page, int size);
    }
}
