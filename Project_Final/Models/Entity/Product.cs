using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Final.Models.Entity
{
    public class Product : BaseDomain
    {
        public long CategoryId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,0)")]
        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public Status Status { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
