namespace ViknCodesTask.Models.ProductModels
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
