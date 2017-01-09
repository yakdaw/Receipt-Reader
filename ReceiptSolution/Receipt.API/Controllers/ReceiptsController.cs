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

    /// <summary>
    /// Receipt based operations
    /// </summary>
    [Authorize]
    public class ReceiptsController : ApiController
    {
        private readonly IReceiptRepository repository;
        private readonly AuthService authService;
        private readonly ResponseService responseService;
        private readonly ControlSumService controlSumService;

        public ReceiptsController(IReceiptRepository repository)
        {
            this.repository = repository;
            this.authService = new AuthService();
            this.responseService = new ResponseService();
            this.controlSumService = new ControlSumService();
        }

        /// <summary>
        /// Get all user receipts
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <response code="200">User receipts successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ReceiptModel>))]
        [Route("api/{userName}/receipts")]
        public HttpResponseMessage GetAllUserReceipts(string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var domainReceipts = this.repository.GetAllUserReceipts(userId);

            var host = Request.RequestUri.Authority;
            var receipts = new List<ReceiptModel>();

            foreach (Receipt domainReceipt in domainReceipts)
            {
                var receipt = new ReceiptModel(domainReceipt, userName, host);
                receipts.Add(receipt);
            }

            return Request.CreateResponse(HttpStatusCode.OK, receipts);
        }

        /// <summary>
        /// Get specified user receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <response code="200">User receipt successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        /// <response code="404">Receipt with given ID not found.</response>
        [HttpGet]
        [ResponseType(typeof(ReceiptModel))]
        [Route("api/{userName}/receipts/{receiptId}")]
        public HttpResponseMessage GetUserReceiptById(string userName, int receiptId)
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
                return Request.CreateResponse(HttpStatusCode.NotFound, "No receipt with id " + receiptId + " for user " + userName);
            }

            var host = Request.RequestUri.Authority;
            var receipt = new ReceiptModel(domainReceipt, userName, host);

            return Request.CreateResponse(HttpStatusCode.OK, receipt);
        }

        /// <summary>
        /// Get specified receipt image
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Receipt ID</param>
        /// <response code="200">User receipt image successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(byte[]))]
        [Route("api/{userName}/receipts/{receiptId}/image")]
        public HttpResponseMessage GetUserReceiptImage(string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);
            var receiptImage = this.repository.GetUserReceiptImage(userId, receiptId);

            return Request.CreateResponse(HttpStatusCode.OK, receiptImage);
        }

        /// <summary>
        /// Add new user receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receipt">New receipt</param>
        /// <response code="200">User receipt successfully added.</response>
        /// <response code="400">Wrong JSON request receipt model.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [Route("api/{userName}/receipts")]
        public IHttpActionResult AddNewUserReceipt(string userName, NewReceiptModel receipt)
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

            if (!controlSumService.ValidateReceiptControlSum(receipt))
            {
                return BadRequest("Wrong control sum for receipt's products.");
            }

            string userId = this.authService.GetUserId(this.User);

            repository.Add(userId, receipt.MapToDomainReceipt());

            return Ok();
        }

        /// <summary>
        /// Update user receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Id of receipt to update</param>
        /// <param name="updatedReceipt">Updated receipt values</param>
        /// <response code="200">User receipt successfully updated.</response>
        /// <response code="400">Wrong JSON request receipt model / No receipt with given id.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPut]
        [Route("api/{userName}/receipts/{receiptId}")]
        public IHttpActionResult UpdateUserReceipt(string userName, int receiptId, UpdatedReceiptModel updatedReceipt)
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
                repository.Update(userId, receiptId, updatedReceipt.MapToDomainReceipt());
            }
            catch (Exception)
            {
                return BadRequest("Receipt was not updated.");
            }

            return Ok();
        }

        /// <summary>
        /// Delete user receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Id of receipt to delete</param>
        /// <response code="200">User receipt successfully deleted.</response>
        /// <response code="400">Wrong JSON request receipt model / No receipt with given id.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpDelete]
        [Route("api/{userName}/receipts/{receiptId}")]
        public IHttpActionResult DeleteUserReceipt(string userName, int receiptId)
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
                repository.Delete(userId, receiptId);
            }
            catch (Exception)
            {
                return BadRequest("Receipt was not deleted.");
            }

            return Ok();
        }

        /// <summary>
        /// Get categories for user receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="receiptId">Id of receipt to delete</param>
        /// <response code="200">User receipt categories successfully sent.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CategoryModel>))]
        [Route("api/{userName}/receipts/{receiptId}/categories")]
        public HttpResponseMessage GetCategoriesForReceipt(string userName, int receiptId)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string userId = this.authService.GetUserId(this.User);

            var domainCategories = this.repository.GetUserReceiptCategories(userId, receiptId);

            if (domainCategories == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No receipt with id " + receiptId + " for user " + userName);
            }

            var host = Request.RequestUri.Authority;
            var categories = new List<CategoryModel>();

            foreach (Category domainCategory in domainCategories)
            {
                var category = new CategoryModel(domainCategory, userName, receiptId, host);
                categories.Add(category);
            }

            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        /// <summary>
        /// Add new empty user receipt
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <response code="200">User receipt successfully added.</response>
        /// <response code="401">No authentication token. / Wrong user name in query.</response>
        [HttpPost]
        [Route("api/{userName}/receipts/newempty")]
        public IHttpActionResult AddNewEmptyUserReceipt(string userName)
        {
            string tokenName = this.authService.GetUserName(this.User);

            if (!tokenName.Equals(userName))
            {
                return Unauthorized();
            }

            string userId = this.authService.GetUserId(this.User);

            repository.AddEmpty(userId);

            return Ok();
        }
    }
}
