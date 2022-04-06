using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace OnlineExamManagementWebApp {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                "DefaultApiWithAction",
                "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                "DefaultApiGet",
                "api/{controller}",
                new { action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
                "DefaultApiPost",
                "api/{controller}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
        }
    }
}
