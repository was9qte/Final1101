using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// Представляет контроллер главной страницы приложения.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Страница с описанием вашего приложения.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ваша страница.";

            return View();
        }
    }
}