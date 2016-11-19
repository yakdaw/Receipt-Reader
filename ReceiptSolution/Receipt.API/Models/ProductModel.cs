﻿namespace Receipt.API.Models
{
    using Domain.Entities;

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }

        public string Url { get; set; }

        public ProductModel(Product domainProduct, string userName, string hostUrl)
        {
            this.Id = domainProduct.Id;
            this.Name = domainProduct.Name;
            this.Price = domainProduct.Price;
            this.Quantity = domainProduct.Quantity;
            this.Category = domainProduct.Category;

            this.Url = hostUrl + "/api/" + userName + "/products/" + Id;
        }
    }
}