using Project_Final.Models.Request;
using Project_Final.Models.Response;

namespace Project_Final.Services
{
    public interface IUserService
    {
        Response<UserResponse> CreateUser(CreateUserRequest createUserRequest);
        Response<AccessTokenReponse> Login(LoginRequest loginRequest);
        Response<UserResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    }
}
