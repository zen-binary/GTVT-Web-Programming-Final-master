using AutoMapper;
using Project_Final.Models;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;

namespace Project_Final.Mappers
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile() 
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<ProductAttribute, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateProductRequest, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, ProductsResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.Join("-", src.Name, src.Size, src.Color)));
        }
    }
}
