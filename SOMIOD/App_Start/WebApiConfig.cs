using System.Web.Http;

namespace SOMIOD
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "SomiodApi",
                routeTemplate: "api/somiod/{application}/{container}/{resource}",
                defaults: new { application = RouteParameter.Optional, 
                    container = RouteParameter.Optional, resource = RouteParameter.Optional }
            );
           

            // remove json
            // https://learn.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/json-and-xml-serialization#removing_the_json_or_xml_formatter
            GlobalConfiguration.Configuration.Formatters.Remove(config.Formatters.JsonFormatter);
            
            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;
      
        }
    }
}