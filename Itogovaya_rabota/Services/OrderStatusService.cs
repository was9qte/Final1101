using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Itogovaya_rabota.Models;

namespace Itogovaya_rabota.Services
{
    public class OrderStatusService
    {
        private const string ConnectionString = @"Data Source=BD.db;Version=3;";

        /// <summary>
        /// Получает список всех статусов заказов.
        /// </summary>
        public List<OrderStatus> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<OrderStatus>("SELECT * FROM OrderStatuses").ToList();
            }
        }
    }

}