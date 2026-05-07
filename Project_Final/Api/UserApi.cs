using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Services;

namespace Project_Final.Api
{
    [ApiController]
    [Route("/admin/api/user")]
    public class UserApi : ControllerBase
    {
        private readonly IUserService userService;

        public UserApi(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public Response<AccessTokenReponse> Login([FromBody] LoginRequest loginRequest)
        {
            return userService.Login(loginRequest);
        }
        [Authorize]
        [HttpPost("create-user")]
        public Response<UserResponse> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            return userService.CreateUser(createUserRequest);
        }

        [Authorize]
        [HttpPost("change-password")]
        public Response<UserResponse> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return userService.ChangePassword(request);
        }

    }
}
