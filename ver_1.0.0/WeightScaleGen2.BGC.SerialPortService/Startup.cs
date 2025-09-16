using Owin;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WeightScaleGen2.BGC.SerialPortService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Enable attribute routing
            config.MapHttpAttributeRoutes();

            // Default route (optional)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            appBuilder.UseWebApi(config);
        }
    }
}
