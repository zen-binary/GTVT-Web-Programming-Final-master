using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Final.Context;
using Project_Final.ExceptionHandle;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_Final.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly DBContext dBContext;
        private readonly IMapper mapper;

        public UserService(DBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }

        public Response<AccessTokenReponse> Login(LoginRequest loginRequest)
        {
            var user = dBContext.Users.FirstOrDefault(u => loginRequest.Username.Equals(u.Username) && loginRequest.Password.Equals(u.Password));
            if (user == null)
                return Response<AccessTokenReponse>.OfFailed(ErrorCode.UNAUTHORIZE, "User " + loginRequest.Username + " invalid");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, loginRequest.Username)
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claimsIdentify = new ClaimsIdentity(claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("utc-shop-final-security-secret-key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentify,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "https://localhost:7055",
                Audience = "https://localhost:7055"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var expriedAt = tokenDescriptor.Expires ?? DateTime.UtcNow;
            return Response<AccessTokenReponse>.OfSuccessed(new AccessTokenReponse() { AccessToken = tokenHandler.WriteToken(token), UserName = loginRequest.Username, ExpriredDate = TimeZoneInfo.ConvertTimeFromUtc(expriedAt, TimeZoneInfo.Local) });
        }

        public Response<UserResponse> CreateUser(CreateUserRequest createUserRequest)
        {
            var fieldValidations = validateUser(createUserRequest);
            if (fieldValidations.Count > 0)
                return Response<UserResponse>.OfFailedFieldValidations(new FieldValidationException(fieldValidations));
            try
            {
                var user = mapper.Map<User>(createUserRequest);
                dBContext.Users.Add(user);
                dBContext.SaveChanges();
                return Response<UserResponse>.OfSuccessed(mapper.Map<UserResponse>(user));
            }
            catch (DbUpdateException ex)
            {
                return Response<UserResponse>.OfFailed(ErrorCode.INVALID_PARAMETER_ERROR, "Tài khoản đã tồn tại");
            }
        }

        private List<FieldValidation> validateUser(CreateUserRequest createUserRequest)
        {
            var fieldValidations = new List<FieldValidation>();
            if (createUserRequest.Username.IsNullOrEmpty())
            {
                fieldValidations.Add(new FieldValidation("Username", "Không được phép null hoặc rỗng"));
            }

            if (createUserRequest.Password.IsNullOrEmpty())
            {
                fieldValidations.Add(new FieldValidation("Password", "Không được phép null hoặc rỗng"));
            }
            return fieldValidations;
        }

        public Response<UserResponse> ChangePassword(ChangePasswordRequest request)
        {
            var user = dBContext.Users.FirstOrDefault(u => request.Username.Equals(u.Username) && request.OldPassword.Equals(u.Password));
            if (user == null)
            {
                return Response<UserResponse>.OfFailed(ErrorCode.UNAUTHORIZE, "User " + request.Username + " không đúng mật khẩu");
            }
            user.Password = request.NewPassword;
            dBContext.Update(user);
            dBContext.SaveChanges();
            return Response<UserResponse>.OfSuccessed();
        }
    }
}
