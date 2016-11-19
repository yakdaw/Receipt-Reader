namespace Receipt.API.Model.EF
{
    using Domain.Entities;
    using Mappers;
    using Model;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System;

    public class ProductRepository : IProductRepository
    {
        private ProductMapper productMapper = null;

        public ProductRepository()
        {
            productMapper = new ProductMapper();
        }

        public Collection<Product> GetAllUserProducts(string userId)
        {
            List<DatabaseModel.Product> products;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                products = db.Product.Where(x => x.Receipt.UserId == userId).ToList();
            }

            var domainProducts = new Collection<Product>();

            foreach (var product in products)
            {
                var domainProduct = productMapper.MapFromDatabase(product);
                domainProducts.Add(domainProduct);
            }

            return domainProducts;
        }

        public Product GetUserProductById(string userId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
