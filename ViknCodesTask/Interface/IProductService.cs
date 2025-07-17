using ViknCodesTask.Common;
using ViknCodesTask.DTOs.ProductsDTOs;

namespace ViknCodesTask.Interface
{
    public interface IProductService
    {
        //Task<ApiResponse<Guid>> CreateProductAsync(ProductCreateDTO dto);
        //Task<ApiResponse<List<ProductDetailsDTO>>> GetProductListAsync();
        //Task<ApiResponse<string>> UpdateStockAsync(UpdateStockDTO dto);
        //Task<ApiResponse<ProductDetailsDTO>> GetProductByIdAsync(Guid id);
        //Task<ApiResponse<Guid>> UpdateProductAsync(UpdateProductDTO dto);

        Task<ApiResponse<Guid>> CreateProductAsync(ProductCreateDTO dto);
        Task<ApiResponse<ProductListResponseDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task<int> AddStockAsync(StockMovementDTO stockDto);
        Task<int> RemoveStockAsync(StockMovementDTO stockDto);

    }
}
