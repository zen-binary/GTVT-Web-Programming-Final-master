using Project_Final.Models;
using Project_Final.Models.Response;

namespace Project_Final.Services
{
    public interface ICardOrderService
    {
        Response<string> Upsert(CardOrder cardOrder);
        Response<List<string>> GetIds();
        Response<CardOrder> GetCardOrder(string id);
        Response<List<CardOrder>> GetCardOrders();
        Response<CardOrder> Delete(string id);
    }
}
