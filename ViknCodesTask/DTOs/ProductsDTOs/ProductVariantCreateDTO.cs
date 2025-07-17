using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductVariantCreateDTO
    {
        [Required, MaxLength(100)]
        public string VariantName { get; set; }

        [Required, MinLength(1)]
        public List<string> Options { get; set; } = new();
    }
}
