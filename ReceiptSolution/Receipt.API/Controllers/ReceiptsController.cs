namespace Receipt.API.Controllers
{
    using Model;
    using Receipt.Domain.Entities;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System;

    /// <summary>
    /// Receipt based operations
    /// </summary>
    [Authorize]
    public class ReceiptsController : ApiController
    {
        private readonly AuthService authService;

        public ReceiptsController(IReceiptRepository repository)
        {
            //this.repository = repository;
            this.authService = new AuthService();
        }

        /// <summary>
        /// Get all user receipts
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <response code="200">User receipts successfully sent.</response>
        /// <response code="400">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Receipt>))]
        [Route("api/{userName}/receipts")]
        public HttpResponseMessage GetAllUserReceipts(HttpRequestMessage request, string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            return request.CreateResponse(HttpStatusCode.OK, this.GetHardCodedReceipts());

            //return this.repository.GetAllUserProducts(userId);
        }

        private object GetHardCodedReceipts()
        {
            var receipts = new List<Receipt>()
            {
                new Receipt()
                {
                    Id = 1,
                    UserId = "abcdef",
                    AddDate = DateTime.Now,
                    PurchaseDate = DateTime.Now,
                    Image = new byte[0],
                    ControlSum = 45.00m,
                    Url = "http://receiptsolution.azurewebsites.net/api/wojtek/receipts/1",
                    Products =
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
                    }   
                },
                new Receipt()
                {
                    Id = 2,
                    UserId = "abcdef",
                    AddDate = DateTime.Now,
                    PurchaseDate = DateTime.Now,
                    Image = new byte[0],
                    ControlSum = 45.00m,
                    Url = "http://receiptsolution.azurewebsites.net/api/wojtek/receipts/2",
                    Products =
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
                        }
                    }
                }
            };

            return receipts;
        }

        /// <summary>
        /// Get specified user receipt
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <response code="200">User receipt successfully sent.</response>
        /// <response code="400">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(Receipt))]
        [Route("api/{userName}/receipts/{receiptId}")]
        public HttpResponseMessage GetUserReceipt(HttpRequestMessage request, string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            return request.CreateResponse(HttpStatusCode.OK, this.GetHardCodedReceipt());

            //return this.repository.GetAllUserProducts(userId);
        }

        private object GetHardCodedReceipt()
        {
            return new Receipt()
            {
                Id = 1,
                UserId = "abcdef",
                AddDate = DateTime.Now,
                PurchaseDate = DateTime.Now,
                Image = new byte[0],
                ControlSum = 45.00m,
                Url = "http://receiptsolution.azurewebsites.net/api/wojtek/receipts/1",
                Products =
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
                }
            };
        }
    }
}
