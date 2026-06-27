using System.Collections.Generic;

namespace Itogovaya_rabota.Models
{
    /// <summary>
    /// Хранит текущий заказ пользователя.
    /// </summary>
    public static class Cart
    {
        public static List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}