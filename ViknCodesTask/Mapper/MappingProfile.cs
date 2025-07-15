using AutoMapper;
using ViknCodesTask.Models;
using ViknCodesTask.DTOs;
using ViknCodesTask.DTOs.ProductsDTOs;
using ViknCodesTask.DTOs.AuthDTOs;
using System.Text;

namespace ViknCodesTask.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();

            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                   src.ProductImage != null
                       ? Encoding.UTF8.GetString(src.ProductImage)
                       : null))
                .ForMember(dest => dest.TotalStock,
                opt => opt.MapFrom(src => src.Variants
                    .SelectMany(v => v.Options)
                    .Sum(opt => opt.Stock)));

            CreateMap<ProductVariant, ProductVariantDTO>();
            CreateMap<SubVariant, ProductVariantOptionDTO>();


            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();

        }
    }
}
