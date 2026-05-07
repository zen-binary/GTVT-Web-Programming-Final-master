namespace Project_Final.Models.Request
{
    public class UpdateCategoryRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
    }
}
