namespace UJConnect.Models
{
    public class ProductPackage
    {
        public int PackageID { get; private set; }
        public Product Product { get; private set; }
        public string PackageName { get; private set; }
        public int NumUnits { get; private set; }
        public decimal Price { get; private set; }

        //default package
        public ProductPackage(Product product, decimal price)
        {
            this.PackageName = "Single Unit";
            this.NumUnits = 1;
            this.Price = price;
            this.Product = product;
        }

        //constructor for adding
        public ProductPackage(Product product, string packageName, int numUnits, decimal price)
        {
            PackageName = packageName;
            Product = product;
            NumUnits = numUnits;
            Price = price;
        }

        //constuctor for accessing
        public ProductPackage(int packageID, Product product, string packageName, int numUnits, decimal price)
        {
            this.PackageID = packageID;
            this.PackageName = packageName;
            this.Product = product;
            this.NumUnits = numUnits;
            this.Price = price;
        }

    }

}
