namespace Receipt.API.Model.EF.Mappers
{
    using Domain.Entities;

    public class CustomizedProductMapper
    {
        public DatabaseModel.CustomizedProduct MapToDatabase(Product domainProduct)
        {
            if (domainProduct == null)
            {
                return null;
            }

            var dbCustomizedProduct = new DatabaseModel.CustomizedProduct();
            dbCustomizedProduct.Name = domainProduct.Name;
            dbCustomizedProduct.CategoryId = domainProduct.Category.Id;

            return dbCustomizedProduct;
        }
    }
}
