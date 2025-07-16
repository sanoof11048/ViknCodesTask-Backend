using ViknCodesTask.Common;

namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductDetailsDTO : BaseEntity
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public bool IsFavourite { get; set; }
        public bool Active { get; set; }
        public string HSNCode { get; set; }
        public int TotalStock { get; set; }

        public List<ProductVariantDTO> Variants { get; set; } = new();
        public List<ProductStockDTO> VariantStocks { get; set; } = new();
    }
}
