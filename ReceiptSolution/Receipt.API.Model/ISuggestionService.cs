namespace Receipt.API.Model
{
    using Domain.Models;

    public interface ISuggestionService
    {
        ProductWithCategory SuggestProductCategoryId(string userId, string productName, string purchasePlace);
    }
}
