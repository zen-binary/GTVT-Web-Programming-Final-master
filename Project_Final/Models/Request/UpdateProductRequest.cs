namespace Project_Final.Models.Request
{
    public class UpdateProductRequest
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Status Status { get; set; }
    }
}
