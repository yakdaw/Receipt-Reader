namespace Receipt.Web.Services
{
    using RestSharp;
    using System.Web;

    public class AuthorizationService
    {
        public RestRequest GenerateAuthorizedRequest(string url, Method httpMethod, HttpContextBase httpContext)
        {
            var userName = httpContext.Session["username"];
            var access = httpContext.Session["access_token"];

            var request = new RestRequest("api/" + userName + url, httpMethod);
            request.AddHeader("Authorization", "Bearer " + access);

            return request;
        }
    }
}