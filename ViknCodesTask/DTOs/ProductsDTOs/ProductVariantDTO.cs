using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductVariantDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string? VariantName { get; set; }
        [Required]
        public List<string>? Options { get; set; }
    }
}
