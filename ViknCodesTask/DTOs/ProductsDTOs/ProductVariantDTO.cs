using System.ComponentModel.DataAnnotations;
using ViknCodesTask.DTOs.ProductsDTOs.ViknCodesTask.DTOs.ProductsDTOs;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductVariantDTO
    {
        [Required]
        public string? VariantName { get; set; }
        [Required]
        public List<ProductVariantOptionDTO>? Options { get; set; } = new();
    }
}
