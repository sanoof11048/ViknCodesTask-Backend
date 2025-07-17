using AutoMapper;
using ViknCodesTask.Models;
using ViknCodesTask.DTOs;
using ViknCodesTask.DTOs.ProductsDTOs;
using ViknCodesTask.DTOs.AuthDTOs;
using System.Text;
using ViknCodesTask.Models.ProductModels;

namespace ViknCodesTask.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();

            CreateMap<Product, ProductResponseDTO>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                src.ProductImage != null ? Encoding.UTF8.GetString(src.ProductImage) : null
            ));

            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
        }
    }
}
