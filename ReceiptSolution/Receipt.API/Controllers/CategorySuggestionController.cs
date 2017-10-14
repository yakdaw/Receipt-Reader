namespace Receipt.API.Controllers
{
    using Domain.Models;
    using Model;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class CategorySuggestionController : ApiController
    {
        private ISuggestionService suggestionService;
        private readonly AuthService authService;

        public CategorySuggestionController(ISuggestionService suggestionService)
        {
            this.suggestionService = suggestionService;
            this.authService = new AuthService();
        }

        /// <summary>
        /// Suggest category for product
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="categorySuggestion">Product name and purchase place</param>
        /// <response code="200">Successfully suggested product with category ID.</response>
        /// <response code="400">Wrong JSON request model.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [ResponseType(typeof(CategorySuggestionResponse))]
        [Route("api/{userName}/category/suggest")]
        public HttpResponseMessage Suggest(string userName, CategorySuggestionModel categorySuggestion)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request model");
            }

            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            var productWithCategory = suggestionService.SuggestProductCategoryId
                (userId, categorySuggestion.ProductName, categorySuggestion.PurchasePlace);

            var response = new CategorySuggestionResponse(productWithCategory);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Suggest products with categories for products list
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="categorySuggestions">Products names and purchase places</param>
        /// <response code="200">Successfully suggested products with category IDs.</response>
        /// <response code="400">Wrong JSON request model.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [ResponseType(typeof(IEnumerable<CategorySuggestionResponse>))]
        [Route("api/{userName}/category/suggestMultiple")]
        public HttpResponseMessage SuggestMultiple(string userName, IEnumerable<CategorySuggestionModel> categorySuggestions)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request model");
            }

            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            var categoryIds = new List<CategorySuggestionResponse>();

            foreach (var categorySuggestion in categorySuggestions)
            {
                var productWithCategory = suggestionService.SuggestProductCategoryId
                    (userId, categorySuggestion.ProductName, categorySuggestion.PurchasePlace);

                categoryIds.Add(new CategorySuggestionResponse(productWithCategory));
            }

            return Request.CreateResponse(HttpStatusCode.OK, categoryIds);
        }
    }
}
