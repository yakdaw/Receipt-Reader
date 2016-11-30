namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Model;
    using Models;
    using Services;
    using System;
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
        private readonly ResponseService responseService;

        /// <summary>
        /// Product based operations
        /// </summary>
        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
            this.authService = new AuthService();
            this.responseService = new ResponseService();
        }

        /// <summary>
        /// Get all user products
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <response code="200">User products successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductModel>))]
        [Route("api/{userName}/products")]
        public HttpResponseMessage GetAllUserProducts(string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainProducts = this.repository.GetAllUserProducts(userId);

            var host = Request.RequestUri.Authority;
            var products = new List<ProductModel>();

            foreach (Product domainProduct in domainProducts)
            {
                var product = new ProductModel(domainProduct, userName, host);
                products.Add(product);
            }

            return Request.CreateResponse(HttpStatusCode.OK, products);
        }

        /// <summary>
        /// Get specified receipt products
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <response code="200">User products successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        /// <response code="404">Receipt with specified ID not found.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductModel>))]
        [Route("api/{userName}/receipts/{receiptId}/products")]
        public HttpResponseMessage GetUserProductsByReceipt(string userName, int receiptId)
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
                return Request.CreateResponse(HttpStatusCode.NotFound, "No receipt with id " + receiptId + " for user " + userName);
            }

            var host = Request.RequestUri.Authority;
            var products = new List<ProductModel>();

            foreach (Product domainProduct in domainProducts)
            {
                var product = new ProductModel(domainProduct, userName, host);
                products.Add(product);
            }

            return Request.CreateResponse(HttpStatusCode.OK, products);
        }

        /// <summary>
        /// Get specified user product
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <param name="productId">Product ID</param>
        /// <response code="200">User product successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        /// <response code="404">Product with specified ID not found.</response>
        [HttpGet]
        [ResponseType(typeof(ProductModel))]
        [Route("api/{userName}/receipts/{receiptId}/products/{productId}")]
        public HttpResponseMessage GetUserProductById(string userName, int receiptId, int productId)
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
                return Request.CreateResponse(HttpStatusCode.NotFound, "No product with id " + productId 
                    + " in receipt " + receiptId + " for user " + userName);
            }

            var host = Request.RequestUri.Authority;
            var product = new ProductModel(domainProduct, userName, host);

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        /// <summary>
        /// Add new user product to receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt id</param>
        /// <param name="product">New product</param>
        /// <response code="200">User product successfully added.</response>
        /// <response code="400">Wrong JSON request product model. / Wrong receipt ID.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [Route("api/{userName}/receipts/{receiptId}/products")]
        public IHttpActionResult AddNewProductToReceipt(string userName, int receiptId, NewProductModel product)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return Unauthorized();
            }

            string userId = this.authService.GetUserId(this.User);

            repository.Add(userId, receiptId, product.MapToDomainProduct());

            return Ok();
        }

        /// <summary>
        /// Update user product
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <param name="productId">ID of product to update</param>
        /// <param name="updatedProduct">Updated product values</param>
        /// <response code="200">User product in receipt successfully updated.</response>
        /// <response code="400">Wrong JSON request product model / No product with given ID in receipt.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPut]
        [Route("api/{userName}/receipts/{receiptId}/products/{productId}")]
        public IHttpActionResult UpdateProductInReceipt(string userName, int receiptId, int productId, UpdatedProductModel updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return Unauthorized();
            }

            string userId = this.authService.GetUserId(this.User);

            try
            {
                repository.Update(userId, receiptId, productId, updatedProduct.MapToDomainProduct());
            }
            catch (Exception)
            {
                return BadRequest("Product was not updated.");
            }

            return Ok();
        }

        /// <summary>
        /// Delete user product
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <param name="productId">ID of product to delete</param>
        /// <response code="200">User product successfully deleted.</response>
        /// <response code="400">Wrong JSON request receipt model / No receipt with given id.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpDelete]
        [Route("api/{userName}/receipts/{receiptId}/products/{productId}")]
        public IHttpActionResult DeleteProductInReceipt(string userName, int receiptId, int productId)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return Unauthorized();
            }

            string userId = this.authService.GetUserId(this.User);

            try
            {
                repository.Delete(userId, receiptId, productId);
            }
            catch (Exception)
            {
                return BadRequest("Product was not deleted.");
            }

            return Ok();
        }
    }
}
