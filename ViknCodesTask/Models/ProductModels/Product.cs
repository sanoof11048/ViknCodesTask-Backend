    using System.ComponentModel.DataAnnotations;
    using ViknCodesTask.Common;

    namespace ViknCodesTask.Models.ProductModels
    {
        public class Product : BaseEntity
        {
            public Guid Id { get; set; }
            [Required, MaxLength(100)]
            public string? ProductCode { get; set; }
            [Required, MaxLength(150)]
            public string? ProductName { get; set; }
            public byte[]? ProductImage { get; set; }
            public bool IsFavourite { get; set; }
            public bool Active { get; set; }
            public string? HSNCode { get; set; }
            public int TotalStock { get; set; }

            public ICollection<ProductVariant>? Variants { get; set; }
            public ICollection<ProductCategory> ProductCategories { get; set; }
        }
    }
