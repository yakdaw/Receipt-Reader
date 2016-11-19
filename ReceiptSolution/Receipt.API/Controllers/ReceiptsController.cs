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

            return request.CreateResponse(HttpStatusCode.OK, this.repository.GetAllUserReceipts(userId));
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
        public HttpResponseMessage GetUserReceiptById(HttpRequestMessage request, string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            return request.CreateResponse(HttpStatusCode.OK, this.repository.GetUserReceiptById(userId, receiptId));
        }
    }
}
