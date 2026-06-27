using System.Web.Mvc;

namespace BookStore.Web
{
    /// <summary>
    /// Содержит методы для регистрации глобальных фильтров приложения.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Регистрирует глобальные фильтры приложения.
        /// </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}