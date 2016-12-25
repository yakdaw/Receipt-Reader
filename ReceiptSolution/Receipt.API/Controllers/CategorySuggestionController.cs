namespace Receipt.API.Controllers
{
    using Model;
    using Models;
    using Services;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class CategorySuggestionController : ApiController
    {
        private readonly ISuggestionService suggestionService;
        private readonly AuthService authService;

        public CategorySuggestionController(ISuggestionService suggestionService)
        {
            this.suggestionService = suggestionService;
            this.authService = new AuthService();
        }

        [HttpPost]
        [ResponseType(typeof(int))]
        [Route("api/{userName}/category/suggest")]
        public HttpResponseMessage SuggestCategory(string userName, CategorySuggestionModel categorySuggestion)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            var categoryId = suggestionService.SuggestProductCategoryId
                (userId, categorySuggestion.ProductName, categorySuggestion.PurchasePlace);

            return Request.CreateResponse(HttpStatusCode.OK, categoryId);
        }
    }
}
