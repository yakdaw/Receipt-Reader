namespace Receipt.API.Controllers
{
    using Model;
    using Domain.Entities;
    using Services;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Models;

    /// <summary>
    /// Receipt based operations
    /// </summary>
    [Authorize]
    public class ReceiptsController : ApiController
    {
        private readonly IReceiptRepository repository;
        private readonly AuthService authService;

        public ReceiptsController(IReceiptRepository repository)
        {
            this.repository = repository;
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
        [ResponseType(typeof(IEnumerable<ReceiptModel>))]
        [Route("api/{userName}/receipts")]
        public HttpResponseMessage GetAllUserReceipts(HttpRequestMessage request, string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainReceipts = this.repository.GetAllUserReceipts(userId);

            var host = request.RequestUri.Authority;
            var receipts = new List<ReceiptModel>();

            foreach (Receipt domainReceipt in domainReceipts)
            {
                var receipt = new ReceiptModel(domainReceipt, userName, host);
                receipts.Add(receipt);
            }

            return request.CreateResponse(HttpStatusCode.OK, receipts);
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
        [ResponseType(typeof(ReceiptModel))]
        [Route("api/{userName}/receipts/{receiptId}")]
        public HttpResponseMessage GetUserReceiptById(HttpRequestMessage request, string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainReceipt = this.repository.GetUserReceiptById(userId, receiptId);

            if (domainReceipt == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, "No receipt with id " + receiptId + " for user " + userName);
            }

            var host = request.RequestUri.Authority;
            var receipt = new ReceiptModel(domainReceipt, userName, host);

            return request.CreateResponse(HttpStatusCode.OK, receipt);
        }

        /// <summary>
        /// Get specified receipt image
        /// </summary>
        /// <param name="request">Request with bearer token authentication for specified user</param>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <response code="200">User receipt image successfully sent.</response>
        /// <response code="400">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(byte[]))]
        [Route("api/{userName}/receipts/{receiptId}/image")]
        public HttpResponseMessage GetUserReceiptImage(HttpRequestMessage request, string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var receiptImage = this.repository.GetUserReceiptImage(userId, receiptId);

            return request.CreateResponse(HttpStatusCode.OK, receiptImage);
        }
    }
}
