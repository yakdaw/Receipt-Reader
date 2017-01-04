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
        [Route("")]
        public ActionResult GetAllUserProducts()
        {
            var userName = HttpContext.Session["username"];
            var access = HttpContext.Session["access_token"];

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = new RestRequest("api/" + userName + "/products/" , Method.GET);

            request.AddHeader("Authorization", "Bearer " + access);

            var response = client.Execute<List<ProductModel>>(request).Data;

            return View(response);
        }

        [HttpGet]
        [Route("chart")]
        public ActionResult GetChartData()
        {
            var userName = HttpContext.Session["username"];
            var access = HttpContext.Session["access_token"];

            var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
            var request = new RestRequest("api/" + userName + "/products/", Method.GET);

            request.AddHeader("Authorization", "Bearer " + access);

            var responseProducts = client.Execute<List<ProductModel>>(request).Data;

            var chartData = chartService.GenerateChartModelFromProductsList(responseProducts);

            return View(chartData);
        }
    }
}