using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Final.Models.Request
{
    public class CreateOrderDetailRequest
    {
        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }


        [Column(TypeName = "decimal(10,0)")]
        public decimal Price { get; set; }
    }
}
