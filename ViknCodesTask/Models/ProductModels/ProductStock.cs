using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.Models.ProductModels
{
    public class ProductStock
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string VariantKey { get; set; }

        public int Stock { get; set; }
    }
}
