using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class CreateProductDTO
    {
        [Required]
        public string? ProductCode { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? HSNCode { get; set; }
        public bool IsFavourite { get; set; }
        public bool Active { get; set; } = true;
        public List<ProductVariantDTO>? Variants { get; set; }
    }
}
