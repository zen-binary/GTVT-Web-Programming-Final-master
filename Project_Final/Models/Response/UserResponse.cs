namespace Project_Final.Models.Response
{
    public class UserResponse
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public List<string> Roles { get; set; }
    }
}
