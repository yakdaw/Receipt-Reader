﻿namespace Receipt.API.Models
{
    using Domain.Entities;
    using System.ComponentModel.DataAnnotations;

    public class NewProductModel
    {
        [Required]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative number")]
        [Display(Name = "Product price")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be higher than 0")]
        [Display(Name = "Product quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Product category ID")]
        public int CategoryId { get; set; }

        public Product MapToDomainProduct()
        {
            var domainProduct = new Product();

            domainProduct.Name = this.Name;
            domainProduct.Price = this.Price;
            domainProduct.Quantity = this.Quantity;

            domainProduct.Category = new Category { Id = this.CategoryId };

            return domainProduct;
        }
    }
}