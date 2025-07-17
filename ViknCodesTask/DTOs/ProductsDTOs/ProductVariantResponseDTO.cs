namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductVariantResponseDTO
    {
        public Guid Id { get; set; }
        public string VariantName { get; set; }
        public List<ProductSubVariantDTO> SubVariants { get; set; } = new();
    }
}
