using System.ComponentModel.DataAnnotations;
using ViknCodesTask.Common;

namespace ViknCodesTask.Models.ProductModels
{
    public class ProductStock : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductVariantId { get; set; }
        public MovementType MovementType { get; set; }
        public ProductVariant? Variant { get; set; }
        public string Notes { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}

public enum MovementType
{
    Purchase,
    Sale
}
