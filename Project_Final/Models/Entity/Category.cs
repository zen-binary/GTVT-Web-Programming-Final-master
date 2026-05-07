using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Final.Models.Entity
{
    public class Category : BaseDomain
    {
        public string Name { get; set; }
        public Status Status { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
