﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViknCodesTask.Models.ProductModels
{
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        [Required, MaxLength(100)]
        public string? VariantName { get; set; }
        public Product Product { get; set; }
        public ICollection<ProductSubVariant>? SubVariants { get; set; }
        public ICollection<ProductStock> ProductStock { get; set; }
    }
}
