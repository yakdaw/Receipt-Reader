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

                    return RedirectToRoute("Receipts");
                }
                else
                {
                    ViewBag.WrongUsernameMessage = "Wrong username or password";
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
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
                    return View("~/Views/Account/AccountRegistered.cshtml");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.WrongMessage = "Username or e-mail already exists in database";
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        [Route("lostpassword", Name = "LostPassword")]
        public ActionResult LostPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("lostpassword", Name = "SendLostPassword")]
        public ActionResult LostPassword(LostPasswordModel lostPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = new RestRequest("api/account/lostpassword", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(lostPasswordModel);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return View("~/Views/Account/LostPasswordSent.cshtml");
                }
            }

            return View();
        }

        [HttpGet]
        [Route("resetpassword", Name = "ResetPassword")]
        public ActionResult ResetPassword(string email, string token)
        {
            var passwordResetModel = new PasswordResetModel();
            passwordResetModel.Token = token;

            return View(passwordResetModel);
        }

        [HttpPost]
        [Route("resetpassword", Name = "SendResetPasswordCredentials")]
        public ActionResult ResetPassword(PasswordResetModel passwordResetModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient(WebConfigurationManager.AppSettings["webApiUrl"]);
                var request = new RestRequest("api/account/resetpassword", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(passwordResetModel);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("ResetPassword",
                        new { email = passwordResetModel.Email, token = passwordResetModel.Token });
                }
            }

            return RedirectToAction("ResetPassword",
                new { email = passwordResetModel.Email, token = passwordResetModel.Token });
        }
    }
}