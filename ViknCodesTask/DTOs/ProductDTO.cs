namespace ViknCodesTask.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string? HSNCode { get; set; }
        public bool IsFavourite { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl {  get; set; }
        public decimal TotalStock { get; set; }
    }
}
