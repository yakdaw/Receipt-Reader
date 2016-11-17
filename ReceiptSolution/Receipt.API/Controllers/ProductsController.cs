namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class ProductsController : ApiController
    {
        //private readonly IProductRepository repository;
        private readonly AuthService authService;

        /// <summary>
        /// Product based operations
        /// </summary>
        public ProductsController(IProductRepository repository)
        {
            //this.repository = repository;
            this.authService = new AuthService();
        }


        /// <summary>
        /// Get all user products
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <response code="200">User products successfully sent.</response>
        /// <response code="400">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Product>))]
        [Route("api/{userName}/products")]
        public HttpResponseMessage GetAllProducts(HttpRequestMessage request, string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            return request.CreateResponse(HttpStatusCode.OK, this.GetHardcodedProducts());

            //return this.repository.GetAllUserProducts(userId);
        }

        /// <summary>
        /// Get specified user product
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <param name="productId">Product ID</param>
        /// <response code="200">User product successfully sent.</response>
        /// <response code="400">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(Product))]
        [Route("api/{userName}/products/{productId}")]
        public HttpResponseMessage GetAllProducts(HttpRequestMessage request, string userName, int productId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            return request.CreateResponse(HttpStatusCode.OK, this.GetHardcodedProduct());

            //return this.repository.GetAllUserProducts(userId);
        }

        private Product GetHardcodedProduct()
        {
            return new Product()
            {
                Id = 1,
                ReceiptId = 1,
                Name = "Serek",
                Price = 3.99m,
                Quantity = 3,
                Category = "Spożywcze",
                Url = "http://receiptsolution.azurewebsites.net/api/wojtek/products/1"
            };
        }

        private IEnumerable<Product> GetHardcodedProducts()
        {
            var products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    ReceiptId = 1,
                    Name = "Serek",
                    Price = 3.99m,
                    Quantity = 3,
                    Category = "Spożywcze",
                    Url = "http://receiptsolution.azurewebsites.net/api/wojtek/products/1"
                },
                new Product()
                {
                    Id = 2,
                    ReceiptId = 1,
                    Name = "Wino",
                    Price = 19.99m,
                    Quantity = 1,
                    Category = "Alkohol",
                    Url = "http://receiptsolution.azurewebsites.net/api/wojtek/products/2"
                }
            };

            return products;
        }
    }
}
