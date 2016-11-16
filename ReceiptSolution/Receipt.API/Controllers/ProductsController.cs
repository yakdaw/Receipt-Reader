namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Model;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System;
    using System.Web.Http.Description;

    public class ProductsController : ApiController
    {
        //private readonly IProductRepository repository;
        private readonly AuthService authService; 

        public ProductsController(IProductRepository repository)
        {
            //this.repository = repository;
            this.authService = new AuthService();
        }

        [HttpGet]
        [Authorize]
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

        [HttpGet]
        [Authorize]
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
                Category = "Spożywcze"
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
                    Category = "Spożywcze"
                },
                new Product()
                {
                    Id = 2,
                    ReceiptId = 1,
                    Name = "Wino",
                    Price = 19.99m,
                    Quantity = 1,
                    Category = "Alkohol"
                }
            };

            return products;
        }
    }
}
