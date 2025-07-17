using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class StockMovementDTO
    {
        [Required]
        public Guid ProductVariantId { get; set; }

        [Required]
        public MovementType MovementType { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string? Notes { get; set; }
    }
}
