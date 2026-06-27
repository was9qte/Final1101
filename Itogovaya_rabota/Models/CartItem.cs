namespace Itogovaya_rabota.Models
{
    /// <summary>
    /// Товар, добавленный в корзину.
    /// </summary>
    public class CartItem
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
}