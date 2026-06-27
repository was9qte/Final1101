using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookStore.Web
{
    /// <summary>
    /// Представляет главное приложение ASP.NET MVC.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Выполняет настройку приложения при его запуске.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}