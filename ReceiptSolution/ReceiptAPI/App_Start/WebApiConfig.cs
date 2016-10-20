using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Autofac;
using Receipt.API.Model.EF;
using Receipt.API.Model;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace Receipt.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerRequest();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
