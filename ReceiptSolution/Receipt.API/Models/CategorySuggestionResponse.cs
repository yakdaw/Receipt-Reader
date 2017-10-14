namespace Receipt.API.Models
{
    using Receipt.Domain.Models;
    using System.ComponentModel.DataAnnotations;

    public class CategorySuggestionResponse
    {
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }

        public CategorySuggestionResponse(ProductWithCategory productWithCategory)
        {
            this.ProductName = productWithCategory.ProductName;
            this.CategoryId = productWithCategory.CategoryId;
        }
    }
}