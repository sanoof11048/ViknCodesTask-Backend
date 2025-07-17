using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ViknCodesTask.Common;
using ViknCodesTask.Data;
using ViknCodesTask.DTOs;
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
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, IMapper mapper, ICloudinaryService cloudinaryService, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        public async Task<ApiResponse<Guid>> CreateProductAsync(ProductCreateDTO dto)
        {
            try
            {
              

                var existingProduct = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductCode == dto.ProductCode);

                if (existingProduct != null)
                {
                    return new ApiResponse<Guid>(409, $"ProductCode '{dto.ProductCode}' already exists.");
                }

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

                if (dto.CategoryIds != null && dto.CategoryIds.Any())
                {
                    product.ProductCategories = new List<ProductCategory>();
                    foreach (var categoryId in dto.CategoryIds)
                    {
                        var category = await _context.ProductCategories.FirstOrDefaultAsync(f => f.Id == categoryId);
                        if (category != null)
                        {
                            product.ProductCategories.Add(category);
                        }
                    }
                }

                product.Variants = new List<ProductVariant>();

                foreach (var variantDto in dto.Variants)
                {
                    var variant = new ProductVariant
                    {
                        Id = Guid.NewGuid(),
                        VariantName = variantDto.VariantName,
                        ProductId = product.Id,
                        SubVariants = new List<ProductSubVariant>()
                    };

                    foreach (var option in variantDto.Options)
                    {
                        variant.SubVariants.Add(new ProductSubVariant
                        {
                            Id = Guid.NewGuid(),
                            OptionName = option,
                            CurrentStock = 0,
                            Variant = variant
                        });
                    }

                    product.Variants.Add(variant);
                }

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();


                return new ApiResponse<Guid>(201, "Product created successfully", product.Id);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Guid>(500, "Error creating product", default, ex.Message);
            }
        }



        public async Task<ApiResponse<ProductListResponseDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return new ApiResponse<ProductListResponseDTO>(400, "Page number and size must be positive integers");
                }

                var query = _context.Products
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.SubVariants)
                    .Include(p => p.ProductCategories)
                    .AsNoTracking();

                var totalCount = await query.CountAsync();
                var products = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var productDtos = _mapper.Map<List<ProductResponseDTO>>(products);
                var response = new ProductListResponseDTO
                {
                    Products = productDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return new ApiResponse<ProductListResponseDTO>(200, "Products retrieved successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return new ApiResponse<ProductListResponseDTO>(500, "Internal server error", null, ex.Message);
            }
        }

        public async Task<int> AddStockAsync(StockMovementDTO stockDto)
        {
            try
            {
                var variant = await _context.ProductVariants
    .Include(v => v.SubVariants)
    .FirstOrDefaultAsync(v => v.Id == stockDto.ProductVariantId);

                if (variant == null)
                {
                    throw new KeyNotFoundException($"Variant with ID {stockDto.ProductVariantId} not found");
                }

                // Update sub-variants stock
                foreach (var subVariant in variant.SubVariants)
                {
                    subVariant.CurrentStock += stockDto.Quantity;
                    _context.ProductSubVariants.Update(subVariant);
                }

                // Update product total stock
                var product = await _context.Products.FirstOrDefaultAsync(f => f.Id == variant.ProductId);
                product.TotalStock += stockDto.Quantity;
                _context.Products.Update(product);

                // Record stock movement
                var stockMovement = new ProductStock
                {
                    Id = Guid.NewGuid(),
                    ProductVariantId = variant.Id,
                    MovementType = stockDto.MovementType,
                    Quantity = stockDto.Quantity,
                    Notes = stockDto.Notes,
                    
                };
                await _context.ProductStocks.AddAsync(stockMovement);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Stock added for variant {VariantId}", variant.Id);
                return product.TotalStock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock");
                throw;
            }
        }

        public async Task<int> RemoveStockAsync(StockMovementDTO stockDto)
        {
            try
            {
                var variant = await _context.ProductVariants
    .Include(v => v.SubVariants)
    .FirstOrDefaultAsync(v => v.Id == stockDto.ProductVariantId);

                if (variant == null)
                {
                    throw new KeyNotFoundException($"Variant with ID {stockDto.ProductVariantId} not found");
                }

                // Check stock availability
                if (variant.SubVariants.Any(sv => sv.CurrentStock < stockDto.Quantity))
                {
                    throw new InvalidOperationException("Insufficient stock for one or more sub-variants");
                }

                // Update sub-variants stock
                foreach (var subVariant in variant.SubVariants)
                {
                    subVariant.CurrentStock -= stockDto.Quantity;
                    _context.ProductSubVariants.Update(subVariant);
                }

                // Update product total stock
                var product = await _context.Products.FirstOrDefaultAsync(f => f.Id == variant.ProductId);
                product.TotalStock -= stockDto.Quantity;

                _context.Products.Update(product);

                // Record stock movement
                var stockMovement = new ProductStock
                {
                    Id = Guid.NewGuid(),
                    ProductVariantId = variant.Id,
                    MovementType = stockDto.MovementType,
                    Quantity = stockDto.Quantity,
                    Notes = stockDto.Notes,
                };
                await _context.ProductStocks.AddAsync(stockMovement);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Stock removed for variant {VariantId}", variant.Id);
                return product.TotalStock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing stock");
                throw;
            }
        }
    }
}
