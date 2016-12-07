namespace Receipt.Web.Controllers
{
    using System.Web.Mvc;

    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("register")]
        public ActionResult Register()
        {
            return View();
        }

        [Route("lostpassword")]
        public ActionResult LostPassword()
        {
            return View();
        }
    }
}