using Itogovaya_rabota.Models;
using Itogovaya_rabota.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Itogovaya_rabota
{
    /// <summary>
    /// Окно корзины.
    /// </summary>
    public partial class CartWindow : Window
    {
        private readonly CartStorageService _cartStorageService = new CartStorageService();

        public CartWindow()
        {
            InitializeComponent();

            CartDataGrid.ItemsSource = null;
            CartDataGrid.ItemsSource = Cart.Items;

            UpdateTotal();

        }

        /// <summary>
        /// Обновляет итоговую стоимость заказа.
        /// </summary>
        private void UpdateTotal()
        {
            decimal total = Cart.Items.Sum(x => x.TotalPrice);

            TotalTextBlock.Text = $"Сумма заказа: {total} руб.";
        }

        /// <summary>
        /// Удаляет выбранный товар из корзины.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items.Count == 0)
            {
                MessageBox.Show(
                    "Корзина пуста",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (CartDataGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите товар для удаления",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    "Удалить выбранный товар из корзины?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            CartItem item = CartDataGrid.SelectedItem as CartItem;

            if (item == null)
            {
                return;
            }

            Cart.Items.Remove(item);

            _cartStorageService.SaveCart(Properties.Settings.Default.UserId);

            CartDataGrid.ItemsSource = null;
            CartDataGrid.ItemsSource = Cart.Items;

            UpdateTotal();

            if (Cart.Items.Count == 0)
            {
                if (MainWindow.Instance != null)
                {
                    MainWindow.Instance.CartButton.Visibility = Visibility.Collapsed;
                }

                MessageBox.Show(
                    "Корзина пуста",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Обрабатывает изменение количества товара.
        /// </summary>
        private void CartDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CartItem item = e.Row.Item as CartItem;

                if (item == null)
                {
                    return;
                }

                if (item.Quantity <= 0)
                {
                    Cart.Items.Remove(item);

                    CartDataGrid.ItemsSource = null;
                    CartDataGrid.ItemsSource = Cart.Items;

                    if (Cart.Items.Count == 0 && MainWindow.Instance != null)
                    {
                        MainWindow.Instance.CartButton.Visibility = Visibility.Collapsed;
                    }
                }

                UpdateTotal();

                _cartStorageService.SaveCart(Properties.Settings.Default.UserId);
            }));
        }

        /// <summary>
        /// Оформляет заказ.
        /// </summary>
        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items.Count == 0)
            {
                MessageBox.Show(
                    "Корзина пуста",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            OrderService orderService = new OrderService();
            OrderItemService orderItemService = new OrderItemService();
            UserService userService = new UserService();

            Random random = new Random();

            User user = null;

            if (Properties.Settings.Default.IsAuthorized)
            {
                user = userService.GetById(Properties.Settings.Default.UserId);
            }

            string articles = string.Join(", ", Cart.Items.Select(x => $"{x.Product.Article}, {x.Quantity}"));

            Order order = new Order
            {
                OrderNumber = orderService.GetNextOrderNumber(), Articles = articles,
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                DeliveryDate = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"),
                UserId = user != null ? user.Id : 0,
                UserName = user != null ? user.FullName : "",
                PickupCode = random.Next(100, 999).ToString(),
                StatusId = 1,
                StatusName = "Новый"
            };

            orderService.Add(order);

            int orderId = orderService.GetLastOrderId();
            int orderNumber = order.OrderNumber;

            StringBuilder ticket = new StringBuilder();

            ticket.AppendLine("ТАЛОН НА ПОЛУЧЕНИЕ");
            ticket.AppendLine();
            ticket.AppendLine($"Дата заказа: {order.OrderDate}");
            ticket.AppendLine($"Номер заказа: {orderNumber}");
            ticket.AppendLine($"Код получения: {order.PickupCode}");
            ticket.AppendLine();
            ticket.AppendLine("Состав заказа:");

            decimal total = 0;

            foreach (CartItem item in Cart.Items)
            {
                orderItemService.Add(
                    new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });

                ticket.AppendLine($"{item.Product.Name} x{item.Quantity}");

                total += item.TotalPrice;
            }

            ticket.AppendLine();
            ticket.AppendLine($"Сумма заказа: {total} руб.");

            string books = string.Join("\n", Cart.Items.Select(x => $"{x.Product.Name} x{x.Quantity}"));

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt", FileName = $"Заказ_{orderNumber}.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(
                    dialog.FileName,
                    ticket.ToString(),
                    Encoding.UTF8);
            }

            MessageBox.Show(
                $"Заказ успешно оформлен!\n\n" +
                $"Книги:\n{books}\n\n" +
                $"Номер заказа: {orderNumber}\n" +
                $"Код получения: {order.PickupCode}",
                "Оформление заказа",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            Cart.Items.Clear();

            _cartStorageService.SaveCart(Properties.Settings.Default.UserId);

            if (MainWindow.Instance != null)
            {
                MainWindow.Instance.CartButton.Visibility = Visibility.Collapsed;
            }

            Close();
        }

        /// <summary>
        /// Закрывает окно корзины.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
