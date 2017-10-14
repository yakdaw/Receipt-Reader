namespace Receipt.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProductWithCategory
    {
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }
    }
}
