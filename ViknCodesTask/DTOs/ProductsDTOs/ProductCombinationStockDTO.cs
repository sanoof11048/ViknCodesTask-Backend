namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class ProductCombinationStockDTO
    {
        public Dictionary<string, string> Combination { get; set; } = new();  // e.g. { "Size": "M", "Color": "Red" }
        public int Stock { get; set; }
    }
}
