using System.ComponentModel.DataAnnotations;

namespace Project_Final.Models.Entity
{
    public abstract class BaseDomain
    {
        [Key]
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }

    }
}
