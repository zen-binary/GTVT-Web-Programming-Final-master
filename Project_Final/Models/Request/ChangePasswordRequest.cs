namespace Project_Final.Models.Request
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
