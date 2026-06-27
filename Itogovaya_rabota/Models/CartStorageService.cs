using Itogovaya_rabota.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Itogovaya_rabota.Services
{
    /// <summary>
    /// Сервис сохранения корзины пользователя.
    /// </summary>
    public class CartStorageService
    {
        private string GetFileName(int userId)
        {
            return $"Cart_{userId}.txt";
        }

        public void SaveCart(int userId)
        {
            List<string> lines = new List<string>();

            foreach (CartItem item in Cart.Items)
            {
                lines.Add(item.Product.Id + ";" + item.Quantity);
            }

            File.WriteAllLines(GetFileName(userId), lines);
        }

        public void LoadCart(int userId, ProductService productService)
        {
            Cart.Items.Clear();

            string file = GetFileName(userId);

            if (!File.Exists(file))
            {
                return;
            }

            string[] lines = File.ReadAllLines(file);

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');

                int productId = int.Parse(parts[0]);
                int quantity = int.Parse(parts[1]);

                Product product = productService.GetById(productId);

                if (product != null)
                {
                    Cart.Items.Add(new CartItem
                    {
                        Product = product,
                        Quantity = quantity
                    });
                }
            }
        }

        public void ClearCart(int userId)
        {
            string file = GetFileName(userId);

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            Cart.Items.Clear();
        }
    }
}