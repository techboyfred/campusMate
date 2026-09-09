namespace UJConnect.Models
{
    public class ProductCardViewModel
    {
        public Product Product { get; set; }
        public decimal LowestPrice { get; set; }

        public ProductCardViewModel(Product product, decimal lowestPrice)
        {
            Product = product;
            LowestPrice = lowestPrice;
        }
    }
}