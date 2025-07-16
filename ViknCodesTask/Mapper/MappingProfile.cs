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
            // Product Mappings
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();

            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.ProductImage != null ? Encoding.UTF8.GetString(src.ProductImage) : null))
                    .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(src => src.TotalStock));

            // Variants
            CreateMap<ProductVariant, ProductVariantDTO>()
    .ForMember(dest => dest.Options, opt => opt.MapFrom(src =>
        src.Options.Select(o => o.Value).ToList()));


            CreateMap<ProductStock, ProductStockDTO>().ReverseMap();


            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
        }
    }
}
