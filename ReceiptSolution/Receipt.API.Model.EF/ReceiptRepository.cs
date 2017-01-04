namespace Receipt.API.Model.EF
{
    using System.Collections.ObjectModel;
    using Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Mappers;
    using System;

    public class ReceiptRepository : IReceiptRepository
    {
        private ReceiptMapper receiptMapper = null;
        private ProductMapper productMapper = null;
        private CategoryMapper categoryMapper = null;
        private CustomizedProductService customizedProductService = null;

        public ReceiptRepository()
        {
            receiptMapper = new ReceiptMapper();
            productMapper = new ProductMapper();
            categoryMapper = new CategoryMapper();
            customizedProductService = new CustomizedProductService();
        }

        public Collection<Receipt> GetAllUserReceipts(string userId)
        {
            List<DatabaseModel.Receipt> receipts;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                receipts = db.Receipt.Where(r => r.UserId == userId).ToList();
            }

            var domainReceipts = new Collection<Receipt>();

            foreach (var receipt in receipts)
            {
                var domainReceipt = receiptMapper.MapFromDatabase(receipt);
                domainReceipts.Add(domainReceipt);
            }

            return domainReceipts;
        }

        public Receipt GetUserReceiptById(string userId, int receiptId)
        {
            DatabaseModel.Receipt receipt;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                receipt = db.Receipt.FirstOrDefault(r => (r.UserId == userId) && (r.Id == receiptId));
            }

            var domainReceipt = receiptMapper.MapFromDatabase(receipt);

            return domainReceipt;
        }

        public byte[] GetUserReceiptImage(string userId, int receiptId)
        {
            byte[] imageBytes = null;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var receipt = db.Receipt.FirstOrDefault(r => (r.UserId == userId) && (r.Id == receiptId));

                if (receipt != null)
                {
                    imageBytes = receipt.Image;
                }
            }

            return imageBytes;
        }

        public void Add(string userId, Receipt receipt)
        {
            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var databaseReceipt = receiptMapper.MapToDatabase(receipt);
                databaseReceipt.Id = GenerateReceiptIdForUser(db, userId);
                databaseReceipt.UserId = userId;
                databaseReceipt.AddDate = DateTime.Now;

                db.Receipt.Add(databaseReceipt);

                int count = 1;
                foreach (Product product in receipt.Products)
                {
                    var databaseProduct = productMapper.MapToDatabase(product);
                    databaseProduct.Id = count;
                    databaseProduct.UserId = userId;
                    databaseProduct.ReceiptId = databaseReceipt.Id;

                    db.Product.Add(databaseProduct);
                    count++;
                }

                db.SaveChanges();
            }

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                foreach (Product product in receipt.Products)
                {
                    if (customizedProductService.CheckForExisting(product, userId, receipt.PurchasePlace, db) == false)
                    {
                        var dbCustomizedProduct = customizedProductService.MapToDatabase(product);
                        dbCustomizedProduct.Id = customizedProductService.GenerateId(userId, db);
                        dbCustomizedProduct.UserId = userId;
                        dbCustomizedProduct.PurchasePlace = receipt.PurchasePlace;

                        db.CustomizedProduct.Add(dbCustomizedProduct);
                    }
                }
                
                // TUTAJ JE BUG
                db.SaveChanges();
            }
        }

        public void Update(string userId, int receiptId, Receipt updatedReceipt)
        {
            if (updatedReceipt == null)
            {
                throw new ArgumentNullException("updatedReceipt");
            }

            var dbUpdatedReceipt = receiptMapper.MapToDatabase(updatedReceipt);
            dbUpdatedReceipt.UserId = userId;
            dbUpdatedReceipt.Id = receiptId;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                db.Receipt.Attach(dbUpdatedReceipt);
                var entry = db.Entry(dbUpdatedReceipt);

                entry.Property(e => e.PurchaseDate).IsModified = true;
                entry.Property(e => e.PurchasePlace).IsModified = true;

                db.SaveChanges();
            }
        }

        public void Delete(string userId, int receiptId)
        {
            var receipt = new DatabaseModel.Receipt()
            {
                Id = receiptId,
                UserId = userId
            };

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                db.Product.RemoveRange(db.Product.Where(p => p.UserId == userId && p.ReceiptId == receiptId));

                db.Receipt.Attach(receipt);
                db.Receipt.Remove(receipt);

                db.SaveChanges();
            }
        }

        private int GenerateReceiptIdForUser(DatabaseModel.ReceiptReaderDatabaseContext db, string userId)
        {
            var query = db.Receipt.Where(r => r.UserId == userId);

            if (query.Count() == 0)
            {
                return 1;
            }

            var lastMax = query.Max(r => r.Id);
            return lastMax + 1;
        }

        public Collection<Category> GetUserReceiptCategories(string userId, int receiptId)
        {
            var domainCategories = new Collection<Category>();

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var receipt = db.Receipt.FirstOrDefault(r => (r.UserId == userId) && (r.Id == receiptId));

                if (receipt == null)
                {
                    return null;
                }

                var dbCategories = new List<DatabaseModel.Category>();

                foreach (var dbProduct in receipt.Product)
                {
                    dbCategories.Add(dbProduct.Category);
                }

                var distinctDbCategories = dbCategories.Distinct();

                foreach (var dbCategory in distinctDbCategories)
                {
                    domainCategories.Add(categoryMapper.MapFromDatabase(dbCategory));
                }   
            }

            return domainCategories;
        }
    }
}
