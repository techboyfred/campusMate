namespace UJConnect.Models
{
    public class Product
    {
        public int ProductID { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public DateTime AddedAt { get; private set; }
        public ProductType ProductType { get; private set; }
        public User AddedByUser { get; private set; }
        public string ImagePath { get; private set; }

        //Constructor for adding a product
        public Product(string productName, string productDescription, ProductType productType,
            User addedByUser, string imagePath) {
            ProductName = productName;
            ProductDescription = productDescription;
            ProductType = productType;
            AddedByUser = addedByUser;
            ImagePath = imagePath;

        }

        //constructor for accessing the product
        public Product(int productID, string productName, string productDescription,
            DateTime dateCreated, ProductType productType, User addedByUser, string imagePath)
        {
            this.ProductID = productID;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.AddedAt = dateCreated;
            this.ProductType = productType;
            this.AddedByUser = addedByUser;
            this.ImagePath = imagePath;
            }
    }
}
