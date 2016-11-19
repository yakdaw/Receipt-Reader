namespace Receipt.API.Model.EF
{
    using Domain.Entities;
    using Mappers;
    using Model;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ProductRepository : IProductRepository
    {
        private ProductMapper productMapper = null;

        public ProductRepository()
        {
            productMapper = new ProductMapper();
        }

        public Collection<Product> GetAllUserProducts(string userId)
        {
            List<DatabaseModel.Product> products;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                products = db.Product.Where(p => p.Receipt.UserId == userId).ToList();
            }

            var domainProducts = new Collection<Product>();

            foreach (var product in products)
            {
                var domainProduct = productMapper.MapFromDatabase(product);
                domainProducts.Add(domainProduct);
            }

            return domainProducts;
        }

        public Collection<Product> GetUserProductsByReceipt(string userId, int receiptId)
        {
            List<DatabaseModel.Product> products;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                products = db.Product.Where(p => (p.Receipt.UserId == userId) && (p.ReceiptId == receiptId)).ToList();
            }

            if (products.Count == 0)
            {
                return null;
            }

            var domainProducts = new Collection<Product>();

            foreach (var product in products)
            {
                var domainProduct = productMapper.MapFromDatabase(product);
                domainProducts.Add(domainProduct);
            }

            return domainProducts;
        }

        public Product GetUserProductById(string userId, int productId)
        {
            DatabaseModel.Product product;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                product = db.Product.FirstOrDefault(p => (p.Receipt.UserId == userId) && (p.Id == productId));
            }

            var domainProduct = productMapper.MapFromDatabase(product);

            return domainProduct;
        }


    }
}
