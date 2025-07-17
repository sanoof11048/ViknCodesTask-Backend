using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.Models.ProductModels
{
    public class ProductSubVariant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductVariantId { get; set; }
        public string OptionName { get; set; }
        public decimal CurrentStock { get; set; }
        public ProductVariant Variant { get; set; }
    }
}
