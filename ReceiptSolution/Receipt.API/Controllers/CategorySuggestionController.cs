namespace Receipt.API.Controllers
{
    using Model;
    using Services;
    using System.Web.Http;

    public class CategorySuggestionController : ApiController
    {
        private readonly ISuggestionService suggestionService;
        private readonly AuthService authService;

        public CategorySuggestionController(ISuggestionService suggestionService)
        {
            this.suggestionService = suggestionService;
            this.authService = new AuthService();
        }

        [AllowAnonymous]
        [Route("api/category/suggest/{productName}")]
        public int SuggestCategory(string productName)
        {
            var categoryId = suggestionService.SuggestProductCategoryId(productName);

            return categoryId;
        }
    }
}
