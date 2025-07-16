using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ViknCodesTask.Common;
using ViknCodesTask.Data;
using ViknCodesTask.DTOs.ProductsDTOs;
using ViknCodesTask.Interface;
using ViknCodesTask.Models;
using ViknCodesTask.Models.ProductModels;

namespace ViknCodesTask.Service
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(AppDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ApiResponse<Guid>> CreateProductAsync(CreateProductDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.ProductCode))
                    return new ApiResponse<Guid>(400, "Invalid product data");

                byte[]? imageBytes = null;
                if (dto.ImageFile != null)
                {
                    string imageUrl = await _cloudinaryService.UploadImage(dto.ImageFile);
                    imageBytes = Encoding.UTF8.GetBytes(imageUrl);
                }

                if (dto.Variants == null || !dto.Variants.Any())
                {
                    return new ApiResponse<Guid>(400, "Variants list cannot be null or empty");
                }

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    ProductCode = dto.ProductCode,
                    ProductName = dto.ProductName,
                    HSNCode = dto.HSNCode,
                    IsFavourite = dto.IsFavourite,
                    Active = dto.Active,
                    ProductImage = imageBytes,
                    TotalStock = 0
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();


                foreach (var variantDto in dto.Variants)
                {
                    var variant = new ProductVariant
                    {
                        Id = Guid.NewGuid(),
                        VariantName = variantDto.VariantName,
                        ProductId = product.Id,
                    };

                    await _context.ProductVariants.AddAsync(variant);
                    await _context.SaveChangesAsync();

                    foreach (var sub in variantDto.Options)
                    {
                        var subVariant = new SubVariant
                        {
                            Id = Guid.NewGuid(),
                            Value = sub,
                            ProductVariantId = variant.Id
                        };
                        await _context.VariantOptions.AddAsync(subVariant);

                    }
                }
                await _context.SaveChangesAsync();

                return new ApiResponse<Guid>(201, "Product created successfully", product.Id);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Guid>(500, "Error creating product", default, ex.Message);
            }
        }



        public async Task<ApiResponse<List<ProductDetailsDTO>>> GetProductListAsync()
        {
            try
            {
                var products = await _context.Products
                    .Include(a=>a.Variants).ThenInclude(ab=>ab.Options)
                    .Include(b=>b.VariantStocks)
                .AsNoTracking()
                .ToListAsync();

                var result = _mapper.Map<List<ProductDetailsDTO>>(products);
                return new ApiResponse<List<ProductDetailsDTO>>(200, "Products fetched", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductDetailsDTO>>(500, "Failed to fetch products", null, ex.Message);
            }
        }

        public async Task<ApiResponse<ProductDetailsDTO>> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Variants).ThenInclude(v => v.Options)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return new ApiResponse<ProductDetailsDTO>(404, "Product not found");
                }

                var dto = _mapper.Map<ProductDetailsDTO>(product);
                return new ApiResponse<ProductDetailsDTO>(200, "Product fetched", dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDetailsDTO>(500, "Something went wrong", null, ex.Message);
            }
        }


        public async Task<ApiResponse<string>> UpdateStockAsync(UpdateStockDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.VariantKey))
                    return new ApiResponse<string>(400, "Variant key required");

                var stock = await _context.ProductStocks
                .FirstOrDefaultAsync(x => x.VariantKey == dto.VariantKey);

                if (stock == null)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
                    if (product == null)
                        return new ApiResponse<string>(404, "Product not found");

                    stock = new ProductStock
                    {
                        Id = Guid.NewGuid(),
                        ProductId = _context.Products.FirstOrDefault()?.Id ?? Guid.Empty,
                        VariantKey = dto.VariantKey,
                        Stock = 0
                    };
                    await _context.ProductStocks.AddAsync(stock);
                }
                if (stock.Stock + dto.Delta < 0)
                    return new ApiResponse<string>(400, "Stock cannot be negative");

                stock.Stock += dto.Delta;
                await _context.SaveChangesAsync();

                return new ApiResponse<string>(200, "Stock updated");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Error updating stock", null, ex.Message);
            }
        }

        //public async Task<ApiResponse<Guid>> UpdateProductAsync(UpdateProductDTO dto)
        //{
        //    try
        //    {
        //        var product = await _context.Products
        //            .Include(p => p.Variants).ThenInclude(v => v.Options)
        //            .Include(p => p.VariantCombinations).ThenInclude(vc => vc.SubVariants)
        //            .FirstOrDefaultAsync(p => p.Id == dto.Id);

        //        if (product == null)
        //            return new ApiResponse<Guid>(404, "Product not found");

        //        if (dto.ImageFile != null)
        //        {
        //            string imageUrl = await _cloudinaryService.UploadImage(dto.ImageFile);
        //            product.ProductImage = Encoding.UTF8.GetBytes(imageUrl);
        //        }

        //        product.ProductCode = dto.ProductCode;
        //        product.ProductName = dto.ProductName;
        //        product.HSNCode = dto.HSNCode;
        //        product.IsFavourite = dto.IsFavourite;
        //        product.Active = dto.Active;

        //        _context.VariantOptions.RemoveRange(product.Variants.SelectMany(v => v.Options));
        //        _context.ProductVariants.RemoveRange(product.Variants);
        //        _context.SubVariantCombinations.RemoveRange(product.VariantCombinations.SelectMany(vc => vc.SubVariants));
        //        _context.VariantCombinations.RemoveRange(product.VariantCombinations);
        //        await _context.SaveChangesAsync();

        //        return new ApiResponse<Guid>(200,"",product.Id);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<Guid>(500, "Error updating product", default, ex.Message);
        //    }
        //}

    }
}
