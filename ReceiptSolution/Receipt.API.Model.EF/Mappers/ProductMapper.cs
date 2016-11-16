namespace Receipt.API.Model.EF.Mappers
{
    using Receipt.Domain.Entities;

    public class ProductMapper
    {
        public Product MapFromDatabase(DatabaseModel.Product databaseProduct)
        {
            if (databaseProduct == null)
            {
                return null;
            }

            var domainProduct = new Product();
            domainProduct.Id = databaseProduct.Id;
            domainProduct.ReceiptId = databaseProduct.ReceiptId;
            domainProduct.Name = databaseProduct.Name;
            domainProduct.Price = databaseProduct.Price;
            domainProduct.Quantity = databaseProduct.Quantity;
            domainProduct.Category = databaseProduct.Category;

            return domainProduct;
        }

        public DatabaseModel.Product MapToDatabase(Product domainProduct)
        {
            if (domainProduct == null)
            {
                return null;
            }

            var databaseProduct = new DatabaseModel.Product();
            databaseProduct.Id = domainProduct.Id;
            databaseProduct.ReceiptId = domainProduct.ReceiptId;
            databaseProduct.Name = domainProduct.Name;
            databaseProduct.Price = domainProduct.Price;
            databaseProduct.Quantity = domainProduct.Quantity;
            databaseProduct.Category = domainProduct.Category;

            return databaseProduct;
        }
    }
}
