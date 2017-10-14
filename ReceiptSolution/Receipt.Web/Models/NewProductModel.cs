namespace Receipt.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class NewProductModel
    {
        [Required]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative number")]
        [Display(Name = "Product price")]
        public double Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be higher than 0")]
        [Display(Name = "Product quantity")]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 31, ErrorMessage = "Category must be number between 1 and 31")]
        [Display(Name = "Product category ID")]
        public int CategoryId { get; set; }
    }
}