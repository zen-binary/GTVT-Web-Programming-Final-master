using AutoMapper;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;

namespace Project_Final.Mappers
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<UpdateCategoryRequest, Category>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
