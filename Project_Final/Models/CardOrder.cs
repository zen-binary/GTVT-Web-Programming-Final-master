namespace Project_Final.Models
{
    public class CardOrder
    {
        public CardOrder(string? receiverName, string? phoneNumber, List<CardOrderDetail>? cardOrderDetails)
        {
            ReceiverName = receiverName;
            PhoneNumber = phoneNumber;
            CardOrderDetails = cardOrderDetails;
        }

        public string? Id { get; set; }
        public string? ReceiverName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ReceiverAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CardOrderDetail>? CardOrderDetails { get; set; }
    }
}
