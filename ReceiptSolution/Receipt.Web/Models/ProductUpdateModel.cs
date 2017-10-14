namespace Receipt.Web.Models
{
    using Domain.Entities;
    using System.ComponentModel.DataAnnotations;
    using ViewModels;
    using System;

    public class ProductUpdateModel
    {
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative number")]
        [Display(Name = "Product price")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be higher than 0")]
        [Display(Name = "Product quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Product category")]
        public CategoryModel Category { get; set; }

        public void MapFromApiModel(ProductModel productModel)
        {
            this.Name = productModel.Name;
            this.Price = productModel.Price;
            this.Quantity = productModel.Quantity;

            this.Category = new CategoryModel()
            {
                Id = productModel.Category.Id,
                Name = productModel.Category.Name
            };
        }
    }

    public class CategoryModel
    {
        [Required]
        [Range(1, 31, ErrorMessage = "Category must be number between 1 and 31")]
        [Display(Name = "Product category ID")]
        public int Id { get; set; }

        [Display(Name = "Product name")]
        public string Name { get; set; }

        internal void MapFromApiModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
        }
    }
}