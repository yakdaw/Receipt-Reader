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
        public Collection<Product> GetAll()
        {
            List<DatabaseModel.Products> products;

            using (var db = new ReceiptEntities())
            {
                products = db.Products.ToList();
            }

            var domainProducts = new Collection<Product>();

            foreach (var product in products)
            {
                var domainProduct = Mappers.ProductMapper.MapFrom(product);
                domainProducts.Add(domainProduct);
            }

            return domainProducts;
        }

        public Product GetOneById(int id)
        {
            DatabaseModel.Products product;
            using (var db = new ReceiptEntities())
            {
                product = db.Products.FirstOrDefault(c => c.Id == id);
            }

            return Mappers.ProductMapper.MapFrom(product);
        }
    }
}
