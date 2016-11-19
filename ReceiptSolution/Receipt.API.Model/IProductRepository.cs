namespace Receipt.API.Model
{
    using Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IProductRepository
    {
        Collection<Product> GetAllUserProducts(string userId);
        Collection<Product> GetUserProductsByReceipt(string userId, int receiptId);
        Product GetUserProductById(string userId, int productId);
    }
}
