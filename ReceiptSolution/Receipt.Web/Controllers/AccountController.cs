namespace Receipt.Web.Controllers
{
    using Models;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System.Net;
    using System.Web.Configuration;
    using System.Web.Mvc;

    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("login", Name = "Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login", Name = "SendLoginCredentials")]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = new RestRequest("token", Method.POST);

                request.AddParameter("client_id", "webApp", ParameterType.GetOrPost);
                request.AddParameter("grant_type", "password", ParameterType.GetOrPost);
                request.AddParameter("username", loginModel.Username, ParameterType.GetOrPost);
                request.AddParameter("password", loginModel.Password, ParameterType.GetOrPost);

                var response = client.Execute(request);
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = JObject.Parse(response.Content);

                    var userName = (string)content["userName"];
                    var accessToken = (string)content["access_token"];

                    HttpContext.Session["username"] = userName;
                    HttpContext.Session["access_token"] = accessToken;

                    return RedirectToRoute("ProductsChart");
                }
            }

            return RedirectToAction("Login");
        }

        [Route("register", Name = "Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register", Name = "SendRegisterCredentials")]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = new RestRequest("api/account/register", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(registerModel);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Register");
        }

        [Route("lostpassword")]
        public ActionResult LostPassword()
        {
            return View();
        }
    }
}