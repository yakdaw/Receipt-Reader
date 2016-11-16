namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class ProductsController : ApiController
    {
        private readonly IProductRepository repository;
        private readonly AuthService authService; 

        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
            this.authService = new AuthService();
        }

        [HttpGet]
        [Authorize]
        [Route("api/{userName}/products")]
        public IEnumerable<Product> GetAllProducts(string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                { ReasonPhrase = "Invalid query for user " + userName + "." };
                throw new HttpResponseException(msg);
            }

            string userId = this.authService.GetUserId(this.User);

            return this.repository.GetAllUserProducts(userId);
        }
    }
}
