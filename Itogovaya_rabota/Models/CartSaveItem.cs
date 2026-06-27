namespace Itogovaya_rabota.Models
{
    /// <summary>
    /// Модель для сохранения корзины пользователя.
    /// </summary>
    public class CartSaveItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}