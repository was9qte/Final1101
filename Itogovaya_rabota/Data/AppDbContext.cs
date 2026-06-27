using System.Data.Entity;
using Itogovaya_rabota.Models;

namespace Itogovaya_rabota.Data
{
    /// <summary>
    /// Контекст базы данных приложения.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        : base("BookStoreDb")
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }
    }

}