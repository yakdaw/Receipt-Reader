namespace Receipt.Web.Controllers
{
    using RestSharp;
    using Services;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using ViewModels;

    [RoutePrefix("products")]
    public class ProductsController : Controller
    {
        readonly AuthorizationService authorizationService;
        readonly ChartService chartService;

        public ProductsController()
        {
            authorizationService = new AuthorizationService();
            chartService = new ChartService();
        }

        [HttpGet]
        [Route("", Name = "Products")]
        public ActionResult GetAllUserProducts()
        {
            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest("/products/", Method.GET, HttpContext);

            var response = client.Execute<List<ProductModel>>(request).Data;

            return View(response);
        }

        [HttpGet]
        [Route("chart", Name = "ProductsChart")]
        public ActionResult GetChartData()
        {
            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = authorizationService.GenerateAuthorizedRequest("/products/", Method.GET, HttpContext);

            var responseProducts = client.Execute<List<ProductModel>>(request).Data;

            var chartData = chartService.GenerateChartModelFromProductsList(responseProducts);

            return View(chartData);
        }
    }
}