namespace Receipt.API.Model
{
    public interface ISuggestionService
    {
        int SuggestProductCategoryId(string userId, string productName, string purchasePlace);
    }
}
