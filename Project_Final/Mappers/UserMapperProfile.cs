using AutoMapper;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;

namespace Project_Final.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<CreateUserRequest, User>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, UserResponse>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
