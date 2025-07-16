using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.Models.ProductModels
{
    public class SubVariant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductVariantId { get; set; }
        [Required]
        public string? Value { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
