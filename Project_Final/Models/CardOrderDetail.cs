using System.Text.Json.Serialization;

namespace Project_Final.Models
{
    public class CardOrderDetail
    {
        public CardOrderDetail(long productId, string productName, decimal price, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
