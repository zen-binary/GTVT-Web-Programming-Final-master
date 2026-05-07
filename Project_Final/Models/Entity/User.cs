using System.ComponentModel.DataAnnotations;

namespace Project_Final.Models.Entity
{
    public class User : BaseDomain
    {
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }
        [StringLength(200)]
        public string Address { get; set; }

        public List<string> Roles { get; set; }
    }
}
