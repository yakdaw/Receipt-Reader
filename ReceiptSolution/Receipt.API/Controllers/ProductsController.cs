namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Model;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class ProductsController : ApiController
    {
        private readonly IProductRepository repository;
        private readonly AuthService authService;

        /// <summary>
        /// Product based operations
        /// </summary>
        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
            this.authService = new AuthService();
        }

        /// <summary>
        /// Get all user products
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <response code="200">User products successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductModel>))]
        [Route("api/{userName}/products")]
        public HttpResponseMessage GetAllUserProducts(HttpRequestMessage request, string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainProducts = this.repository.GetAllUserProducts(userId);

            var host = request.RequestUri.Authority;
            var products = new List<ProductModel>();

            foreach (Product domainProduct in domainProducts)
            {
                var product = new ProductModel(domainProduct, userName, host);
                products.Add(product);
            }

            return request.CreateResponse(HttpStatusCode.OK, products);
        }

        /// <summary>
        /// Get specified receipt products
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <response code="200">User products successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        /// <response code="404">Receipt with specified ID not found.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductModel>))]
        [Route("api/{userName}/receipts/{receiptId}/products")]
        public HttpResponseMessage GetUserProductsByReceipt(HttpRequestMessage request, string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainProducts = this.repository.GetUserProductsByReceipt(userId, receiptId);

            if (domainProducts == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "No receipt with id " + receiptId + " for user " + userName);
            }

            var host = request.RequestUri.Authority;
            var products = new List<ProductModel>();

            foreach (Product domainProduct in domainProducts)
            {
                var product = new ProductModel(domainProduct, userName, host);
                products.Add(product);
            }

            return request.CreateResponse(HttpStatusCode.OK, products);
        }

        /// <summary>
        /// Get specified user product
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <param name="productId">Product ID</param>
        /// <response code="200">User product successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        /// <response code="404">Product with specified ID not found.</response>
        [HttpGet]
        [ResponseType(typeof(ProductModel))]
        [Route("api/{userName}/receipts/{receiptId}/products/{productId}")]
        public HttpResponseMessage GetUserProductById(HttpRequestMessage request, string userName, int receiptId, int productId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainProduct = this.repository.GetUserProductById(userId, receiptId, productId);

            if (domainProduct == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "No product with id " + productId 
                    + " in receipt " + receiptId + " for user " + userName);
            }

            var host = request.RequestUri.Authority;
            var product = new ProductModel(domainProduct, userName, host);

            return request.CreateResponse(HttpStatusCode.OK, product);
        }
    }
}
