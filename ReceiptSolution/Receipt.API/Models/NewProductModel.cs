namespace Receipt.API.Models
{
    using Domain.Entities;
    using System.ComponentModel.DataAnnotations;

    public class NewProductModel
    {
        [Required]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Product quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Product category")]
        public string Category { get; set; }

        public Product MapToDomainProduct()
        {
            var domainProduct = new Product();

            domainProduct.Name = this.Name;
            domainProduct.Price = this.Price;
            domainProduct.Quantity = this.Quantity;
            domainProduct.Category = this.Category;

            return domainProduct;
        }
    }
}