namespace Receipt.API.Model
{
    using Receipt.Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IProductRepository
    {
        Collection<Product> GetAll(string userName);
    }
}
