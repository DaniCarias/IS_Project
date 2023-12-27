using System.Web.Http;

namespace SOMIOD
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application__Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}