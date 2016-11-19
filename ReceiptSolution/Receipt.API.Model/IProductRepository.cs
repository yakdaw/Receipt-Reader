namespace Receipt.API.Model
{
    using Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IProductRepository
    {
        Collection<Product> GetAllUserProducts(string userId);
        Product GetUserProductById(string userId, int productId);
    }
}
