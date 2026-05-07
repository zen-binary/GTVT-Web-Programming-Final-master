using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Final.Models.Entity
{
    public class OrderDetail : BaseDomain
    {
        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }


        [Column(TypeName = "decimal(10,0)")]
        public decimal Price { get; set; }

        public virtual Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
