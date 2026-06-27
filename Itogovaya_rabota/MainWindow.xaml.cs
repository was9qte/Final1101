using Itogovaya_rabota.Models;
using Itogovaya_rabota.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Itogovaya_rabota
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        private User _currentUser;

        private readonly ProductService _productService = new ProductService();

        private readonly CartStorageService _cartStorageService = new CartStorageService();

        private List<Product> _allProducts;

        /// <summary>
        /// Инициализирует главное окно.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            if (Properties.Settings.Default.IsAuthorized)
            {
                UserNameTextBlock.Text = Properties.Settings.Default.UserName;

                LoginButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Visible;

                if (Properties.Settings.Default.RoleId == 1 || Properties.Settings.Default.RoleId == 2)
                {
                    OrdersButton.Visibility = Visibility.Visible;
                }
            }

            _allProducts = _productService.GetAll();

            ManufacturerComboBox.Items.Add("Все производители");

            foreach (string manufacturer in _allProducts.Select(x => x.ManufacturerName).Distinct().OrderBy(x => x))
            {
                ManufacturerComboBox.Items.Add(manufacturer);
            }

            ManufacturerComboBox.SelectedIndex = 0;
            SortComboBox.SelectedIndex = 0;

            ApplyFilters();
        }

        /// <summary>
        /// Вызывает обновление фильтров.
        /// </summary>
        private void FilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Применяет фильтрацию и сортировку товаров.
        /// </summary>
        private void ApplyFilters()
        {
            IEnumerable<Product> products = _allProducts;

            decimal minPrice = 0;

            if (!string.IsNullOrWhiteSpace(MinPriceTextBox.Text) && !decimal.TryParse(MinPriceTextBox.Text, out minPrice))
            {
                MinPriceTextBox.Clear();

                MessageBox.Show(
                    "'Цена от' должна быть числом",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            decimal maxPrice = 0;

            if (!string.IsNullOrWhiteSpace(MaxPriceTextBox.Text) && !decimal.TryParse(MaxPriceTextBox.Text, out maxPrice))
            {
                MaxPriceTextBox.Clear();

                MessageBox.Show(
                    "'Цена до' должна быть числом",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            string searchText = SearchTextBox.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                products = products.Where(x => x.Name != null && x.Name.ToLower().Contains(searchText));
            }

            if (ManufacturerComboBox.SelectedItem != null && ManufacturerComboBox.SelectedItem.ToString() != "Все производители")
            {
                string manufacturer = ManufacturerComboBox.SelectedItem.ToString();

                products = products.Where(x => x.ManufacturerName == manufacturer);
            }

            if (!string.IsNullOrWhiteSpace(MinPriceTextBox.Text))
            {
                products = products.Where(x => x.Price >= minPrice);
            }

            if (!string.IsNullOrWhiteSpace(MaxPriceTextBox.Text))
            {
                products = products.Where(x => x.Price <= maxPrice);
            }

            if (SortComboBox.SelectedItem is ComboBoxItem sortItem)
            {
                switch (sortItem.Content.ToString())
                {
                    case "Название (A-Я)":
                        products = products.OrderBy(x => x.Name.ToLower());
                        break;

                    case "Название (Я-А)":
                        products = products.OrderByDescending(x => x.Name.ToLower());
                        break;

                    case "Цена (Возрастание)":
                        products = products.OrderBy(x => x.Price);
                        break;

                    case "Цена (Убывание)":
                        products = products.OrderByDescending(x => x.Price);
                        break;
                }
            }

            List<Product> result = products.ToList();

            ProductsListView.ItemsSource = result;

            if (result.Count == 0
                && !string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                CountTextBlock.Text = "По вашему запросу ничего не найдено";
            }
            else
            {
                CountTextBlock.Text = $"{result.Count} из {_allProducts.Count}";
            }
        }

        /// <summary>
        /// Выполняет вход пользователя.
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                _currentUser = loginWindow.CurrentUser;

                _cartStorageService.LoadCart(_currentUser.Id, _productService);

                CartButton.Visibility = Cart.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

                Properties.Settings.Default.UserId = _currentUser.Id;
                Properties.Settings.Default.UserName = _currentUser.FullName;
                Properties.Settings.Default.RoleId = _currentUser.RoleId;
                Properties.Settings.Default.IsAuthorized = true;

                Properties.Settings.Default.Save();

                UserNameTextBlock.Text = _currentUser.FullName;

                LoginButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Visible;

                if (_currentUser.RoleId == 1
                    || _currentUser.RoleId == 2)
                {
                    OrdersButton.Visibility = Visibility.Visible;
                }

                MessageBox.Show(
                    $"Вы вошли в аккаунт: {_currentUser.FullName}",
                    "Успешный вход",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Выполняет выход пользователя.
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show(
                    "Вы действительно хотите выйти из аккаунта?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            _currentUser = null;

            _cartStorageService.SaveCart(Properties.Settings.Default.UserId);

            Cart.Items.Clear();

            CartButton.Visibility = Visibility.Collapsed;

            if (CartButton != null)
            {
                CartButton.Visibility = Visibility.Collapsed;
            }

            Properties.Settings.Default.UserId = 0;
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.RoleId = 0;
            Properties.Settings.Default.IsAuthorized = false;

            Properties.Settings.Default.Save();

            UserNameTextBlock.Text = "Гость";

            LoginButton.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Collapsed;
            OrdersButton.Visibility = Visibility.Collapsed;
            CartButton.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Добавляет книгу в корзину.
        /// </summary>
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (sender as Button)?.DataContext as Product;

            if (product == null)
            {
                return;
            }

            CartItem existingItem = Cart.Items.FirstOrDefault(x => x.Product.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Cart.Items.Add(
                    new CartItem
                    {
                        Product = product,
                        Quantity = 1
                    });
            }

            CartButton.Visibility = Visibility.Visible;

            _cartStorageService.SaveCart(Properties.Settings.Default.UserId);

            MessageBox.Show(
                $"Книга \"{product.Name}\" добавлена в корзину",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Открывает окно заказов.
        /// </summary>
        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow window = new OrdersWindow();

            window.ShowDialog();
        }

        /// <summary>
        /// Открывает окно корзины.
        /// </summary>
        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            CartWindow window = new CartWindow();

            window.ShowDialog();
        }
    }
}
