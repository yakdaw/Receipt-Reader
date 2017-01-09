namespace Receipt.Web.Controllers
{
    using Models;
    using RestSharp;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Net;
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
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest("/receipts/", Method.GET, HttpContext);

            var response = client.Execute<List<ReceiptModel>>(request).Data;

            return View(response);
        }

        [HttpGet]
        [Route("{receiptId}/products", Name = "ReceiptProducts")]
        public ActionResult GetUserReceiptProducts(int receiptId)
        {
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService
                .GenerateAuthorizedRequest("/receipts/" + receiptId + "/products/", Method.GET, HttpContext);

            var response = client.Execute<List<ProductModel>>(request).Data;

            if (response != null)
            {
                return View(response);
            }

            return View("~/Views/Home/NotExist.cshtml");
        }

        [HttpPost]
        [Route("", Name = "AddNewEmptyReceipt")]
        public ActionResult AddEmptyReceipt()
        {
            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest
                ("/receipts/newempty/" , Method.POST, HttpContext);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("");
            }

            return View();
        }

        [HttpGet]
        [Route("{receiptId}/update", Name = "UpdateReceipt")]
        public ActionResult UpdateReceipt(int receiptId)
        {
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest
                ("/receipts/" + receiptId, Method.GET, HttpContext);

            var receiptModel = client.Execute<ReceiptModel>(request).Data;

            if (receiptModel != null)
            {
                var receiptUpdateModel = new ReceiptUpdateModel();
                receiptUpdateModel.MapFromApiModel(receiptModel);

                return View(receiptUpdateModel);
            }

            return View("~/Views/Home/NotExist.cshtml");
        }

        [HttpPost]
        [Route("{receiptId}/update", Name = "SendUpdatedReceipt")]
        public ActionResult UpdateReceipt(int receiptId, ReceiptUpdateModel receiptUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = authorizationService.GenerateAuthorizedRequest
                    ("/receipts/" + receiptId + "/", Method.PUT, HttpContext);

                request.RequestFormat = DataFormat.Json;

                // Temporary fix date due to AddJsonBody issue
                if (receiptUpdateModel.PurchaseDate != null)
                {
                    var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                    receiptUpdateModel.PurchaseDate += offset;
                }
                request.AddJsonBody(receiptUpdateModel);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("");
                }
                else
                {
                    ViewBag.WrongMessage = response.ErrorMessage;
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        [Route("{receiptId}", Name = "DeleteReceipt")]
        public ActionResult DeleteReceipt(int receiptId)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = authorizationService.GenerateAuthorizedRequest
                    ("/receipts/" + receiptId + "/", Method.DELETE, HttpContext);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("");
                }
                else
                {
                    ViewBag.WrongMessage = response.ErrorMessage;
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        [Route("{receiptId}/newproduct", Name = "NewProduct")]
        public ActionResult AddNewProduct(int receiptId)
        {
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService
                .GenerateAuthorizedRequest("/receipts/" + receiptId + "/products/", Method.GET, HttpContext);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return View();
            }

            return View("~/Views/Home/NotExist.cshtml");
        }

        [HttpPost]
        [Route("{receiptId}/newproduct", Name = "PostNewProduct")]
        public ActionResult AddNewProduct(int receiptId, NewProductModel newProductModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = authorizationService.GenerateAuthorizedRequest
                    ("/receipts/" + receiptId + "/products/", Method.POST, HttpContext);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(newProductModel);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("GetUserReceiptProducts");
                }
                else
                {
                    ViewBag.WrongMessage = response.ErrorMessage;
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        [Route("{receiptId}/products/{productId}/update", Name = "UpdateProduct")]
        public ActionResult UpdateProduct(int receiptId, int productId)
        {
            if (HttpContext.Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest
                ("/receipts/" + receiptId + "/products/" + productId, Method.GET, HttpContext);

            var productModel = client.Execute<ProductModel>(request).Data;

            if (productModel != null)
            {
                var productUpdateModel = new ProductUpdateModel();
                productUpdateModel.MapFromApiModel(productModel);

                return View(productUpdateModel);
            }

            return View("~/Views/Home/NotExist.cshtml");
        }

        [HttpPost]
        [Route("{receiptId}/products/{productId}/update", Name = "SendUpdatedProduct")]
        public ActionResult UpdateProduct(int receiptId, int productId, ProductUpdateModel productUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = authorizationService.GenerateAuthorizedRequest
                    ("/receipts/" + receiptId + "/products/" + productId + "/", Method.PUT, HttpContext);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(productUpdateModel);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("GetUserReceiptProducts", new { receiptId = receiptId });
                }
                else
                {
                    ViewBag.WrongMessage = response.ErrorMessage;
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        [Route("{receiptId}/products/{productId}", Name = "DeleteProduct")]
        public ActionResult DeleteProduct(int receiptId, int productId)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = authorizationService.GenerateAuthorizedRequest
                    ("/receipts/" + receiptId + "/products/" + productId + "/", Method.DELETE, HttpContext);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Check if receipt still exists
                    request = authorizationService
                        .GenerateAuthorizedRequest("/receipts/" + receiptId + "/products/", Method.GET, HttpContext);
                    response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("GetUserReceiptProducts", new { receiptId = receiptId });
                    }

                    // If does not exist (last product was deleted)
                    return RedirectToAction("");
                }
                else
                {
                    ViewBag.WrongMessage = response.ErrorMessage;
                    return View();
                }
            }

            return View();
        }
    }
}