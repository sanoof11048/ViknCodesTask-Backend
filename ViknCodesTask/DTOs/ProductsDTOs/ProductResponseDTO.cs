namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductResponseDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFavourite { get; set; }
        public bool Active { get; set; }
        public string? HSNCode { get; set; }
        public int TotalStock { get; set; }
        public List<ProductVariantResponseDTO> Variants { get; set; } = new();
        public List<ProductCategoryDTO> Categories { get; set; } = new();
    }
}
