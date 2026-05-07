namespace Project_Final.Models.Request
{
    public class CreateProductRequest
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductAttribute> Details { get; set; }
    }
}
