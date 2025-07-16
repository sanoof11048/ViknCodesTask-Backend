using ViknCodesTask.Common;
using ViknCodesTask.DTOs.ProductsDTOs;

namespace ViknCodesTask.Interface
{
    public interface IProductService
    {
        Task<ApiResponse<Guid>> CreateProductAsync(CreateProductDTO dto);
        Task<ApiResponse<List<ProductDetailsDTO>>> GetProductListAsync();
        Task<ApiResponse<string>> UpdateStockAsync(UpdateStockDTO dto);
        Task<ApiResponse<ProductDetailsDTO>> GetProductByIdAsync(Guid id);
        //Task<ApiResponse<Guid>> UpdateProductAsync(UpdateProductDTO dto);

    }
}
