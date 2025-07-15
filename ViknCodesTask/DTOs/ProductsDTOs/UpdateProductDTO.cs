using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class UpdateProductDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? ProductCode { get; set; }

        [Required]
        public string? ProductName { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? HSNCode { get; set; }
        public bool IsFavourite { get; set; }
        public bool Active { get; set; }

        public ICollection<ProductVariantDTO>? Variants { get; set; }
    }

}
