using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using UJConnect.Models;

namespace UJConnect.Data
{
    public class ProductDAO
    {
        private readonly String _connectionString;
        private UserDAO userDAO;

        public ProductDAO(String connectionString)
        {
            _connectionString = connectionString;
            userDAO = new UserDAO(connectionString);
        }

        public bool AddProduct(Product product) {
            return true;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            
            //sql query to find ALL products
            string sql = "SELECT * FROM Product";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(sql, connection);

            //execute query
            using var reader = command.ExecuteReader();
            while (reader.NextResult()) {
                Product product = new Product(
                    productID: reader.GetInt32("ProductID"),
                    productName: reader.GetString("ProductName"),
                    productDescription: reader.GetString("Description"),
                    dateCreated: reader.GetDateTime("AddedAt"),
                    productType: Enum.Parse<ProductType>(reader.GetString("ProductType")),
                    addedByUser: userDAO.GetUserByID(reader.GetInt32("AddedByUserID")),
                    imagePath: reader.GetString("ImagePath")
                );
                products.Add(product);
            }

            return products;
        }

        public List<Product> GetProductsByUser(User user)
        {
            List<Product> products = new List<Product>();


            return products;
        }

        public List<Product> GetProductsStillInStock(User user)
        {
            List<Product> products = new List<Product>();


            return products;
        }

        public List<Product> FilterproductsByType(List<Product> products, ProductType productType)
        {
            List<Product> filteredProducts = new List<Product>();

            return filteredProducts;
        }

        public bool EditProductName(Product product, string productName) {
            //sql query to modify name
            string sql = "UPDATE Product SET ProductName = @ProductName WHERE ProductID = @ProductID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductID", product.ProductID);
            command.Parameters.AddWithValue("@ProductName", productName);
            using var reader = command.ExecuteReader();

            string newProductName = reader.GetString("ProductName");

            return productName.Equals(newProductName);
        }

        public bool EditProductType(Product product, ProductType productType)
        {
            //sql query to modify type
            string sql = "UPDATE Product SET ProductType = @ProductType WHERE ProductID = @ProductID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductID", product.ProductID);
            command.Parameters.AddWithValue("@ProductType", productType.ToString());
            using var reader = command.ExecuteReader();

            ProductType newProductType = Enum.Parse<ProductType>(reader.GetString("ProductType"));

            return productType.Equals(newProductType);
        }

        public bool EditProducDescription(Product product, string productDescription)
        {
            //sql query to modify name
            string sql = "UPDATE Product SET ProductDescription = @ProductDescription WHERE ProductID = @ProductID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductID", product.ProductID);
            command.Parameters.AddWithValue("@ProducDescription", productDescription);
            using var reader = command.ExecuteReader();

            string newProductDescription = reader.GetString("ProductDescription");

            return productDescription.Equals(newProductDescription);
        }

        public bool EditImagePath(Product product, string imagePath)
        {
            //sql query to modify name
            string sql = "UPDATE Product SET ImagePath = @ImagePath WHERE ProductID = @ProductID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductID", product.ProductID);
            command.Parameters.AddWithValue("@ImagePath", imagePath);
            using var reader = command.ExecuteReader();

            string newImagePath = reader.GetString("ImagePath");

            return imagePath.Equals(newImagePath);
        }

        public bool removeProduct(int productID) {
            //sql query to delete the product
            string sql = "DELETE FROM Product WHERE ProductID = @ProductID";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductID", productID);
            command.ExecuteNonQuery();

            return true;
        }


    }
}
