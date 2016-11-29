using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Receipt.API.App_Start.Swagger;
using Receipt.API.Providers;
using Swashbuckle.Application;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Receipt.API.Startup))]
namespace Receipt.API
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthServerOptions;
        public static IDataProtectionProvider dataProtectionProvider;

        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            ConfigureDataProtection(app);
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "WebAPI");
                c.OperationFilter<AuthorizationHeaderFilter>();
                c.IncludeXmlComments(GetXmlCommentsPath());
            }).EnableSwaggerUi();

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(6),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public void ConfigureDataProtection(IAppBuilder app)
        {
            dataProtectionProvider = app.GetDataProtectionProvider();
        }

        protected static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\Receipt.API.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}