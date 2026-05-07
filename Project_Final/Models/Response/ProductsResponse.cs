namespace Project_Final.Models.Response
{
    public class ProductsResponse
    {
        public long Id { get; set; }  
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public Status Status { get; set; }
    }
}
