using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ViknCodesTask.Common;
using ViknCodesTask.Data;
using ViknCodesTask.DTOs;
using ViknCodesTask.DTOs.ProductsDTOs;
using ViknCodesTask.Interface;
using ViknCodesTask.Models;

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


                int totalStock = 0;
                var productVariants = new List<ProductVariant>();
                var subVariants = new List<SubVariant>();

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
                            Value = sub.Value,
                            Stock = sub.Stock,
                            ProductVariantId = variant.Id
                        };

                        totalStock += sub.Stock;
                        await _context.VariantOptions.AddAsync(subVariant);
                    }
                }

                await _context.SaveChangesAsync();

                product.TotalStock = totalStock;
                _context.Products.Update(product);
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
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.Options).ToListAsync();

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
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.Options)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return new ApiResponse<ProductDetailsDTO>(404, "Product not found", null, "Found Sme Errors");
                }

                var dto = _mapper.Map<ProductDetailsDTO>(product);
                return new ApiResponse<ProductDetailsDTO>(200, "Product fetched",  dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDetailsDTO>(500, "Something went wrong: " + ex.Message, null, ex.Message);

            }
        }


        public async Task<ApiResponse<string>> UpdateStockAsync(UpdateStockDTO dto)
        {
            try
            {
                var option = await _context.VariantOptions.FirstOrDefaultAsync(o => o.Id == dto.SubVariantId);

                if (option == null)
                {
                    return new ApiResponse<string>(404, "Sub-variant not found", null, "Found Sme Errors");
                }

                var newStock = option.Stock + dto.Quantity;

                if (newStock < 0)
                {
                    return new ApiResponse<string>(400, "Cannot reduce stock below zero");
                }

                option.Stock = newStock;
                _context.VariantOptions.Update(option);

                var product = await _context.Products
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.Options)
                    .FirstOrDefaultAsync(p => p.Variants.Any(v => v.Options.Any(o => o.Id == dto.SubVariantId)));

                if (product != null)
                {
                    product.TotalStock = product.Variants.Sum(v => v.Options.Sum(o => o.Stock));
                }
                await _context.SaveChangesAsync();

                var message = dto.Quantity >= 0
                    ? $"Added {dto.Quantity} units to stock"
                    : $"Removed {Math.Abs(dto.Quantity)} units from stock";

                return new ApiResponse<string>(200, message);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Error updating stock: " + ex.Message, null);
            }
        }

        public async Task<ApiResponse<Guid>> UpdateProductAsync(UpdateProductDTO dto)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.Options)
                    .FirstOrDefaultAsync(p => p.Id == dto.Id);

                if (product == null)
                    return new ApiResponse<Guid>(404, "❌ Product not found");

                if (dto.ImageFile != null)
                {
                    string imageUrl = await _cloudinaryService.UploadImage(dto.ImageFile);
                    product.ProductImage = Encoding.UTF8.GetBytes(imageUrl);
                }

                product.ProductCode = dto.ProductCode;
                product.ProductName = dto.ProductName;
                product.HSNCode = dto.HSNCode;
                product.IsFavourite = dto.IsFavourite;
                product.Active = dto.Active;

                // Remove existing variants & options
                var existingVariants = product.Variants.ToList();
                foreach (var variant in existingVariants)
                {
                    _context.VariantOptions.RemoveRange(variant.Options);
                }
                _context.ProductVariants.RemoveRange(existingVariants);
                await _context.SaveChangesAsync();

                int totalStock = 0;

                // Add new variants
                foreach (var variantDto in dto.Variants)
                {
                    var variant = new ProductVariant
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        VariantName = variantDto.VariantName
                    };

                    await _context.ProductVariants.AddAsync(variant);
                    await _context.SaveChangesAsync();

                    foreach (var opt in variantDto.Options ?? new())
                    {
                        var subVariant = new SubVariant
                        {
                            Id = Guid.NewGuid(),
                            ProductVariantId = variant.Id,
                            Value = opt.Value,
                            Stock = opt.Stock
                        };
                        totalStock += opt.Stock;
                        await _context.VariantOptions.AddAsync(subVariant);
                    }
                }

                product.TotalStock = totalStock;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ApiResponse<Guid>(200, "✅ Product updated successfully", product.Id);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Guid>(500, "❌ Error updating product", default, ex.Message);
            }
        }



    }
}
