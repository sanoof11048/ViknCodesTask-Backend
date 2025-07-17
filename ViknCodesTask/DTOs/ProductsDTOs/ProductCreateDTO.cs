using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductCreateDTO
    {
        [Required]
        public string? ProductCode { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? HSNCode { get; set; }
        public bool IsFavourite { get; set; }
        public bool Active { get; set; } = true;
        [Required]
        public List<ProductVariantCreateDTO> Variants { get; set; } = new();

        public List<Guid>? CategoryIds { get; set; }
    }
}
