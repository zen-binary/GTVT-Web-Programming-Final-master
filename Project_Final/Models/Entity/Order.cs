namespace Project_Final.Models.Entity
{
    public class Order : BaseDomain
    {
        public string? ReceiverName { get; set; }

        public string? ReceiverAddress { get; set; }

        public string? ReceiverPhoneNumber { get; set; }

        public string? Decription { get; set; }

        public OrderStatus Status { get; set; }

        public virtual ICollection<OrderDetail> Details { get; set; }
    }
}
