namespace ViknCodesTask.DTOs.ProductsDTOs
{
    public class UpdateStockDTO
    {
        public Guid ProductId { get; set; } 
        public string VariantKey { get; set; } = string.Empty;
        public int Delta { get; set; }

    }
}
