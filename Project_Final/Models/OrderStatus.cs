namespace Project_Final.Models
{
    public enum OrderStatus
    {
        INIT,
        PROSESSING,
        SUCCESS,
        FAIL,
        CANCEL,
        ALL
    }
    public static class OrderStatusExtensions
    {
        public static List<OrderStatus> ListNextStatus(this OrderStatus orderStatus)
        {
            return orderStatus switch
            {
                OrderStatus.INIT => new List<OrderStatus>() { OrderStatus.PROSESSING, OrderStatus.CANCEL },
                OrderStatus.PROSESSING => new List<OrderStatus>() { OrderStatus.SUCCESS, OrderStatus.FAIL, OrderStatus.CANCEL },
                OrderStatus.SUCCESS => new List<OrderStatus>() { },
                OrderStatus.FAIL => new List<OrderStatus>() { },
                OrderStatus.CANCEL => new List<OrderStatus>() { },
                _ => throw new ArgumentOutOfRangeException(nameof(orderStatus), orderStatus, null)
            };
        }

        public static bool IsNextStatus(OrderStatus oldStatus, OrderStatus nextStatus)
        {
            return ListNextStatus(oldStatus).Contains(nextStatus);
        }
    }
}
