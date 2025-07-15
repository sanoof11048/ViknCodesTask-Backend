using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductVariantDTO
    {
        [Required]
        public string? VariantName { get; set; }
        [Required]
        public List<ProductVariantOptionDTO>? Options { get; set; }
    }
}
