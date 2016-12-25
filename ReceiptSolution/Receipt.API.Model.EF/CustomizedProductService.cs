namespace Receipt.API.Model.EF
{
    using Domain.Entities;
    using Mappers;
    using System.Linq;

    public class CustomizedProductService
    {
        private CustomizedProductMapper customizedProductMapper = null;

        public CustomizedProductService()
        {
            customizedProductMapper = new CustomizedProductMapper();
        }

        public DatabaseModel.CustomizedProduct MapToDatabase(Product product)
        {
            return customizedProductMapper.MapToDatabase(product);
        }

        public bool CheckForExisting(Product product, string userId, string purchasePlace, DatabaseModel.ReceiptReaderDatabaseContext db)
        {
            if (purchasePlace != null)
            {
                return db.CustomizedProduct.Any
                    (cp => cp.UserId == userId && cp.Name == product.Name);
            }
            else
            {
                return db.CustomizedProduct.Any
                    (cp => cp.UserId == userId && cp.PurchasePlace == purchasePlace && cp.Name == product.Name);
            }
        }

        public int GenerateId(string userId, DatabaseModel.ReceiptReaderDatabaseContext db)
        {
            var query = db.CustomizedProduct.Where(p => p.UserId == userId);

            if (query.Count() == 0)
            {
                return 1;
            }

            var lastMax = query.Max(r => r.Id);
            return lastMax + 1;
        }
    }
}
