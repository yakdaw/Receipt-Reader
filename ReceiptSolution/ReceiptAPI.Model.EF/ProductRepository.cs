namespace Receipt.API.Model.EF
{
    using Receipt.Domain.Entities;
    using Receipt.API.Model;
    using System;
    using System.Collections.ObjectModel;

    public class ProductRepository : IProductRepository
    {
        public Collection<Product> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
