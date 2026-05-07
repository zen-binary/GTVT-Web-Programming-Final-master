namespace Project_Final.Models.Response
{
    public class OrderResponse
    {
        public long Id { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverPhoneNumber {  get; set; }
        public string? ReceiverAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Decription { get; set; }
        public OrderStatus Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
