using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TelerikMvcDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            var cloneCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cloneCulture.DateTimeFormat.FullDateTimePattern = "yyyy/MM/dd HH:mm:ss";
            cloneCulture.DateTimeFormat.LongDatePattern = "yyyy/MM/dd";
            cloneCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            cloneCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            cloneCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

            Thread.CurrentThread.CurrentCulture = cloneCulture;
        }
    }
}
