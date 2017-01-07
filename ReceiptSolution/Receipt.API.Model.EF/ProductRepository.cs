namespace Receipt.API.Model.EF
{
    using Domain.Entities;
    using Mappers;
    using Model;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System;

    public class ProductRepository : IProductRepository
    {
        private ProductMapper productMapper = null;
        private CustomizedProductService customizedProductService = null;

        public ProductRepository()
        {
            productMapper = new ProductMapper();
            customizedProductService = new CustomizedProductService();
        }

        public Collection<Product> GetAllUserProducts(string userId)
        {
            var domainProducts = new Collection<Product>();

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var dbProducts = db.Product.Where(p => p.UserId == userId);

                foreach (var product in dbProducts)
                {
                    var domainProduct = productMapper.MapFromDatabase(product);
                    domainProducts.Add(domainProduct);
                }
            }

            return domainProducts;
        }

        public Collection<Product> GetUserProductsByReceipt(string userId, int receiptId)
        {
            var domainProducts = new Collection<Product>();

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var dbProducts = db.Product.Where(p => (p.UserId == userId) && (p.ReceiptId == receiptId));
           
                if (dbProducts.Count() == 0)
                {
                    return null;
                }

                foreach (var dbProduct in dbProducts)
                {
                    var domainProduct = productMapper.MapFromDatabase(dbProduct);
                    domainProducts.Add(domainProduct);
                }
            }

            return domainProducts;
        }

        public Collection<Product> GetUserProductsByReceiptCategory(string userId, int receiptId, int categoryId)
        {
            var domainProducts = new Collection<Product>();

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var dbProducts = db.Product.Where(p => (p.UserId == userId) && (p.ReceiptId == receiptId)
                    && (p.CategoryId == categoryId));

                if (dbProducts.Count() == 0)
                {
                    return null;
                }

                foreach (var dbProduct in dbProducts)
                {
                    var domainProduct = productMapper.MapFromDatabase(dbProduct);
                    domainProducts.Add(domainProduct);
                }
            }

            return domainProducts;
        }

        public Product GetUserProductById(string userId, int receiptId, int productId)
        {
            var domainProduct = new Product();

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var dbProduct = db.Product.FirstOrDefault(p => (p.UserId == userId) && (p.ReceiptId == receiptId)
                    && (p.Id == productId));

                domainProduct = productMapper.MapFromDatabase(dbProduct);
            }

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
                UpdateReceiptControlSum(receiptId, userId, db);

                db.SaveChanges();
            }

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var receipt = db.Receipt.FirstOrDefault(r => r.Id == receiptId);

                if (customizedProductService.CheckForExisting(product, userId, receipt.PurchasePlace, db) == false)
                {
                    var dbCustomizedProduct = customizedProductService.MapToDatabase(product);
                    dbCustomizedProduct.Id = customizedProductService.GenerateId(userId, db);
                    dbCustomizedProduct.UserId = userId;
                    dbCustomizedProduct.PurchasePlace = receipt.PurchasePlace;

                    db.CustomizedProduct.Add(dbCustomizedProduct);

                    db.SaveChanges();
                }
            }
        }

        private void UpdateReceiptControlSum
            (int receiptId, string userId, DatabaseModel.ReceiptReaderDatabaseContext db)
        {
            var receipt = db.Receipt.FirstOrDefault(r => r.UserId == userId && r.Id == receiptId);
            receipt.ControlSum = receipt.Product.Sum(p => p.Price * p.Quantity);
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
                entry.Property(e => e.CategoryId).IsModified = true;

                UpdateReceiptControlSum(receiptId, userId, db);

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

                UpdateReceiptControlSum(receiptId, userId, db);

                db.SaveChanges();
            }
        }
    }
}
