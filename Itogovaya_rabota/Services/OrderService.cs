using Dapper;
using Itogovaya_rabota.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Itogovaya_rabota.Services
{
    /// <summary>
    /// Сервис для работы с заказами.
    /// </summary>
    public class OrderService
    {
        private const string ConnectionString = @"Data Source=BD.db;Version=3;";

        /// <summary>
        /// Получает список всех заказов.
        /// </summary>
        public List<Order> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<Order>("SELECT * FROM Orders").ToList();
            }
        }

        /// <summary>
        /// Получает заказ по идентификатору.
        /// </summary>
        public Order GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QueryFirstOrDefault<Order>("SELECT * FROM Orders WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет новый заказ.
        /// </summary>
        public void Add(Order order)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                INSERT INTO Orders
                (
                    OrderNumber,
                    Articles,
                    OrderDate,
                    DeliveryDate,
                    UserId,
                    UserName,
                    PickupCode,
                    StatusId,
                    StatusName
                )
                VALUES
                (
                    @OrderNumber,
                    @Articles,
                    @OrderDate,
                    @DeliveryDate,
                    @UserId,
                    @UserName,
                    @PickupCode,
                    @StatusId,
                    @StatusName
                );";

                connection.Execute(sql, order);
            }
        }

        /// <summary>
        /// Обновляет данные заказа.
        /// </summary>
        public void Update(Order order)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                UPDATE Orders
                SET
                    OrderNumber = @OrderNumber,
                    Articles = @Articles,
                    OrderDate = @OrderDate,
                    DeliveryDate = @DeliveryDate,
                    UserId = @UserId,
                    UserName = @UserName,
                    PickupCode = @PickupCode,
                    StatusId = @StatusId,
                    StatusName = @StatusName
                WHERE Id = @Id;";

                connection.Execute(sql, order);
            }
        }

        /// <summary>
        /// Удаляет заказ.
        /// </summary>
        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM Orders WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Получает список заказов для отображения.
        /// </summary>
        public List<OrderView> GetOrdersWithStatus()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                const string sql = @"
                SELECT
                    o.Id,
                    o.OrderNumber,
                    o.Articles,
                    o.OrderDate,
                    o.DeliveryDate,
                    o.UserId,
                    CASE
                        WHEN o.UserName IS NULL
                            OR o.UserName = ''
                        THEN u.FullName
                        ELSE o.UserName
                    END AS UserName,
                    o.PickupCode,
                    o.StatusId,
                    s.Name AS StatusName,
                    GROUP_CONCAT(p.Name, ', ') AS ProductName
                FROM Orders o
                LEFT JOIN Users u
                    ON u.Id = o.UserId
                LEFT JOIN OrderStatuses s
                    ON s.Id = o.StatusId
                LEFT JOIN OrderItems oi
                    ON oi.OrderId = o.Id
                LEFT JOIN Products p
                    ON p.Id = oi.ProductId
                GROUP BY
                    o.Id,
                    o.OrderNumber,
                    o.Articles,
                    o.OrderDate,
                    o.DeliveryDate,
                    o.UserId,
                    o.UserName,
                    o.PickupCode,
                    o.StatusId,
                    s.Name,
                    u.FullName
                ORDER BY
                    o.OrderNumber;";

                return connection.Query<OrderView>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает идентификатор последнего заказа.
        /// </summary>
        public int GetLastOrderId()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<int>("SELECT MAX(Id) FROM Orders");
            }
        }

        /// <summary>
        /// Получает следующий номер заказа.
        /// </summary>
        public int GetNextOrderNumber()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                int? maxOrderNumber = connection.QueryFirstOrDefault<int?>("SELECT MAX(OrderNumber) FROM Orders");

                return (maxOrderNumber ?? 0) + 1;
            }
        }
    }
}