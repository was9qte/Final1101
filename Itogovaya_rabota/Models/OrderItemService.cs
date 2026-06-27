using Dapper;
using System.Data.SQLite;
using Itogovaya_rabota.Models;

namespace Itogovaya_rabota.Services
{
    /// <summary>
    /// Сервис для работы с позициями заказа.
    /// </summary>
    public class OrderItemService
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        private const string ConnectionString = @"Data Source=BD.db;Version=3";

        /// <summary>
        /// Добавляет позицию заказа в базу данных.
        /// </summary>
        public void Add(OrderItem item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(
                    @"INSERT INTO OrderItems
                    (OrderId, ProductId, Quantity, Price)
                    VALUES
                    (@OrderId, @ProductId, @Quantity, @Price)",
                    item);
            }
        }
    }
}