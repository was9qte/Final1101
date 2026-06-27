using Dapper;
using Itogovaya_rabota.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Itogovaya_rabota.Services
{
    /// <summary>
    /// Предоставляет методы для работы с товарами.
    /// </summary>
    public class ProductService
    {
        private const string ConnectionString = @"Data Source=BD.db;Version=3;";

        /// <summary>
        /// Получает список всех товаров.
        /// </summary>
        public List<Product> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                SELECT
                    p.*,
                    m.Name AS ManufacturerName
                FROM Products p
                LEFT JOIN Manufacturers m
                    ON p.ManufacturerId = m.Id;";

                return connection.Query<Product>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает товар по идентификатору.
        /// </summary>
        public Product GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                return connection.QueryFirstOrDefault<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет новый товар.
        /// </summary>
        public void Add(Product product)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                INSERT INTO Products
                (
                    Article,
                    Name,
                    Unit,
                    Price,
                    Author,
                    ManufacturerId,
                    CategoryId,
                    Discount,
                    QuantityInStock,
                    Description,
                    ImagePath
                )
                VALUES
                (
                    @Article,
                    @Name,
                    @Unit,
                    @Price,
                    @Author,
                    @ManufacturerId,
                    @CategoryId,
                    @Discount,
                    @QuantityInStock,
                    @Description,
                    @ImagePath
                );";

                connection.Execute(sql, product);
            }
        }

        /// <summary>
        /// Обновляет данные товара.
        /// </summary>
        public void Update(Product product)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                UPDATE Products
                SET
                    Article = @Article,
                    Name = @Name,
                    Unit = @Unit,
                    Price = @Price,
                    Author = @Author,
                    ManufacturerId = @ManufacturerId,
                    CategoryId = @CategoryId,
                    Discount = @Discount,
                    QuantityInStock = @QuantityInStock,
                    Description = @Description,
                    ImagePath = @ImagePath
                WHERE Id = @Id;";

                connection.Execute(sql, product);
            }
        }

        /// <summary>
        /// Удаляет товар.
        /// </summary>
        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute("DELETE FROM Products WHERE Id = @Id", new { Id = id });
            }
        }
    }
}