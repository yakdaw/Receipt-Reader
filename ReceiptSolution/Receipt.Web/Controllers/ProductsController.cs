namespace Receipt.Web.Controllers
{
    using RestSharp;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using ViewModels;

    [RoutePrefix("products")]
    public class ProductsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("get")]
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
    }
}