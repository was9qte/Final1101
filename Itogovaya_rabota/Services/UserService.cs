using Dapper;
using Itogovaya_rabota.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Itogovaya_rabota.Services
{
    /// <summary>
    /// Предоставляет методы для работы с пользователями.
    /// </summary>
    public class UserService
    {
        private const string ConnectionString = @"Data Source=BD.db;Version=3;";

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        public List<User> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                return connection.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        public User GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                return connection.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет нового пользователя.
        /// </summary>
        public void Add(User user)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                INSERT INTO Users
                (
                    FullName,
                    Login,
                    Password,
                    RoleId
                )
                VALUES
                (
                    @FullName,
                    @Login,
                    @Password,
                    @RoleId
                );";

                connection.Execute(sql, user);
            }
        }

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        public void Update(User user)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                UPDATE Users
                SET
                    FullName = @FullName,
                    Login = @Login,
                    Password = @Password,
                    RoleId = @RoleId
                WHERE Id = @Id;";

                connection.Execute(sql, user);
            }
        }

        /// <summary>
        /// Удаляет пользователя.
        /// </summary>
        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                connection.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Выполняет авторизацию пользователя.
        /// </summary>
        public User Login(string login, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                SELECT *
                FROM Users
                WHERE Login = @Login
                    AND Password = @Password;";

                return connection.QueryFirstOrDefault<User>(
                    sql,
                    new
                    {
                        Login = login,
                        Password = password
                    });
            }
        }
    }
}