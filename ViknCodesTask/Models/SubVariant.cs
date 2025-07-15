using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViknCodesTask.Models
{
    public class SubVariant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductVariantId { get; set; }
        [Required]
        public string? Value { get; set; }
        public int Stock { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
