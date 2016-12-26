namespace Receipt.API.Controllers
{
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
        /// <response code="200">Successfully suggested category ID.</response>
        /// <response code="400">Wrong JSON request model.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [ResponseType(typeof(int))]
        [Route("api/{userName}/category/suggest")]
        public HttpResponseMessage SuggestCategory(string userName, CategorySuggestionModel categorySuggestion)
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

            var categoryId = suggestionService.SuggestProductCategoryId
                (userId, categorySuggestion.ProductName, categorySuggestion.PurchasePlace);

            return Request.CreateResponse(HttpStatusCode.OK, categoryId);
        }

        /// <summary>
        /// Suggest categories for products list
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="categorySuggestions">Products names and purchase places</param>
        /// <response code="200">Successfully suggested categories IDs.</response>
        /// <response code="400">Wrong JSON request model.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [ResponseType(typeof(IEnumerable<int>))]
        [Route("api/{userName}/category/suggestMultiple")]
        public HttpResponseMessage SuggestCategories(string userName, IEnumerable<CategorySuggestionModel> categorySuggestions)
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

            var categoryIds = new List<int>();

            foreach (var categorySuggestion in categorySuggestions)
            {
                var categoryId = suggestionService.SuggestProductCategoryId
                    (userId, categorySuggestion.ProductName, categorySuggestion.PurchasePlace);

                categoryIds.Add(categoryId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, categoryIds);
        }
    }
}
