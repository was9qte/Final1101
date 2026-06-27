using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Web
{
    /// <summary>
    /// Содержит методы для регистрации маршрутов приложения.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Регистрирует маршруты приложения.
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
               defaults: new
               {
                   controller = "Product",
                   action = "Index",
                   id = UrlParameter.Optional
               }
            );
        }
    }
}