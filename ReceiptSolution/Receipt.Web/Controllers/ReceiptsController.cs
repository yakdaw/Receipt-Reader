namespace Receipt.Web.Controllers
{
    using RestSharp;
    using Services;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using ViewModels;

    [RoutePrefix("receipts")]
    public class ReceiptsController : Controller
    {
        readonly AuthorizationService authorizationService;

        public ReceiptsController()
        {
            authorizationService = new AuthorizationService();
        }

        [HttpGet]
        [Route("", Name = "Receipts")]
        public ActionResult GetAllUserReceipts()
        {
            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest("/receipts/", Method.GET, HttpContext);

            var response = client.Execute<List<ReceiptModel>>(request).Data;

            return View(response);
        }

        [HttpGet]
        [Route("{receiptId}", Name = "ReceiptProducts")]
        public ActionResult GetUserReceiptProducts(int receiptId)
        {
            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService
                .GenerateAuthorizedRequest("/receipts/" + receiptId + "/products/", Method.GET, HttpContext);

            var response = client.Execute<List<ProductModel>>(request).Data;

            return View(response);
        }
    }
}