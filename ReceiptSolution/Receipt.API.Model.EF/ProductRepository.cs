namespace Receipt.API.Model.EF
{
    using Receipt.Domain.Entities;
    using Receipt.API.Model;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using DatabaseModel;
    using System.Linq;

    public class ProductRepository : IProductRepository
    {
        public Collection<Domain.Entities.Product> GetAll(string userName)
        {
            List<DatabaseModel.Products> products;

            using (var db = new ReceiptEntities())
            {
                products = db.Products.Where(x => x.User == userName).ToList();
            }

            var domainProducts = new Collection<Domain.Entities.Product>();

            foreach (var product in products)
            {
                var domainProduct = Mappers.ProductMapper.MapFrom(product);
                domainProducts.Add(domainProduct);
            }

            return domainProducts;
        }
    }
}
