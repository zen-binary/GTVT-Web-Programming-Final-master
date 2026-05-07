namespace Project_Final.Models.Request
{
    public class CreateOrderRequest
    {
        public string? ReceiverName { get; set; }

        public string? ReceiverAddress { get; set; }

        public string? ReceiverPhoneNumber { get; set; }

        public string? Decription { get; set; }

        public List<CreateOrderDetailRequest> Details { get; set; }
    }
}
