namespace Project_Final.Models.Request
{
    public class CreateUserRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string Address { get; set; }

        public List<string> Roles { get; set; }
    }
}
