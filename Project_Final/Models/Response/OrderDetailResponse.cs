namespace Project_Final.Models.Response
{
    public class OrderDetailResponse
    {
        public long Id { get; set; }
        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
