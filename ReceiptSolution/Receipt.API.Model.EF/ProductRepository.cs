namespace Receipt.API.Model.EF
{
    using Domain.Entities;
    using Mappers;
    using Model;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System;

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
                products = db.Product.Where(p => p.UserId == userId).ToList();
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
                products = db.Product.Where(p => (p.UserId == userId) && (p.ReceiptId == receiptId)).ToList();
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

        public Product GetUserProductById(string userId, int receiptId, int productId)
        {
            DatabaseModel.Product product;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                product = db.Product.FirstOrDefault(p => (p.UserId == userId) && (p.ReceiptId == receiptId)
                    && (p.Id == productId));
            }

            var domainProduct = productMapper.MapFromDatabase(product);

            return domainProduct;
        }

        public void Add(string userId, int receiptId, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var databaseProduct = productMapper.MapToDatabase(product);
                databaseProduct.Id = GenerateProductIdForReceipt(db, userId, receiptId);
                databaseProduct.UserId = userId;
                databaseProduct.ReceiptId = receiptId;

                db.Product.Add(databaseProduct);

                db.SaveChanges();
            }
        }

        private int GenerateProductIdForReceipt(DatabaseModel.ReceiptReaderDatabaseContext db, string userId, int receiptId)
        {
            var query = db.Product.Where(p => p.UserId == userId && p.ReceiptId == receiptId);

            if (query.Count() == 0)
            {
                return 1;
            }

            var lastMax = query.Max(r => r.Id);
            return lastMax + 1;
        }

        public void Update(string userId, int receiptId, int productId, Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                throw new ArgumentNullException("updatedProduct");
            }

            var dbUpdatedProduct = productMapper.MapToDatabase(updatedProduct);
            dbUpdatedProduct.Id = productId;
            dbUpdatedProduct.UserId = userId;
            dbUpdatedProduct.ReceiptId = receiptId;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                db.Product.Attach(dbUpdatedProduct);
                var entry = db.Entry(dbUpdatedProduct);

                entry.Property(e => e.Name).IsModified = true;
                entry.Property(e => e.Price).IsModified = true;
                entry.Property(e => e.Quantity).IsModified = true;
                entry.Property(e => e.Category).IsModified = true;

                db.SaveChanges();
            }
        }

        public void Delete(string userId, int receiptId, int productId)
        {
            var product = new DatabaseModel.Product()
            {
                Id = productId,
                UserId = userId,
                ReceiptId = receiptId
            };

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {

                var productCount = db.Product.Count(p => p.UserId == userId && p.ReceiptId == receiptId);

                db.Product.Attach(product);
                db.Product.Remove(product);

                if (productCount == 1)
                {
                    var receipt = new DatabaseModel.Receipt()
                    {
                        Id = receiptId,
                        UserId = userId
                    };

                    db.Receipt.Attach(receipt);
                    db.Receipt.Remove(receipt);
                }

                db.SaveChanges();
            }
        }
    }
}
