using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using BookStore.Web.Models;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// Представляет контроллер для работы с товарами.
    /// </summary>
    public class ProductController : Controller
    {
        /// <summary>
        /// Отображает список всех товаров.
        /// </summary>
        public ActionResult Index()
        {
            string connectionString = $"Data Source={Server.MapPath("~/bin/BD.db")};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                List<Product> products = connection.Query<Product>("SELECT * FROM Products").ToList();

                return View(products);
            }
        }

        /// <summary>
        /// Отображает страницу оформления заказа выбранного товара.
        /// </summary>
        public ActionResult Order(int id)
        {
            string connectionString = $"Data Source={Server.MapPath("~/bin/BD.db")};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                Product product = connection.QueryFirstOrDefault<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });

                return View(product);
            }
        }
    }
}