namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductListResponseDTO
    {
        public List<ProductResponseDTO> Products { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
