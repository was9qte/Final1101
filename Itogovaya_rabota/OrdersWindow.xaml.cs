using Itogovaya_rabota.Models;
using Itogovaya_rabota.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Itogovaya_rabota
{
    /// <summary>
    /// Окно управления заказами.
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private List<OrderView> _orders;

        public OrdersWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.RoleId != 1 && Properties.Settings.Default.RoleId != 2)
            {
                MessageBox.Show("Недостаточно прав");

                Close();

                return;
            }

            LoadData();
        }

        /// <summary>
        /// Загружает данные о заказах.
        /// </summary>
        private void LoadData()
        {
            OrderService orderService = new OrderService();
            OrderStatusService statusService = new OrderStatusService();

            _orders = orderService.GetOrdersWithStatus();

            OrdersDataGrid.ItemsSource = _orders;

            DataGridComboBoxColumn statusColumn = OrdersDataGrid.Columns.OfType<DataGridComboBoxColumn>().FirstOrDefault();

            if (statusColumn != null)
            {
                statusColumn.ItemsSource = statusService.GetAll();
            }
        }

        /// <summary>
        /// Выполняет поиск заказа по номеру.
        /// </summary>
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OrderNumberTextBox.Text))
            {
                MessageBox.Show(
                    "Введите номер заказа",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(OrderNumberTextBox.Text, out int number))
            {
                MessageBox.Show(
                    "Номер заказа должен содержать только цифры",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            List<OrderView> result = _orders.Where(x => x.OrderNumber == number).ToList();

            if (result.Count == 0)
            {
                MessageBox.Show(
                    "Заказ с таким номером не найден",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            OrdersDataGrid.ItemsSource = result;
        }

        /// <summary>
        /// Запрещает ввод нечисловых символов.
        /// </summary>
        private void OrderNumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");

            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Проверяет вставляемый текст.
        /// </summary>
        private void OrderNumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));

                if (!text.All(char.IsDigit))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Сохраняет изменения заказов.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            OrderService service = new OrderService();

            bool hasChanges = false;

            foreach (OrderView item in _orders)
            {
                Order order = service.GetById(item.Id);

                if (order.DeliveryDate != item.DeliveryDate || order.StatusId != item.StatusId)
                {
                    hasChanges = true;

                    break;
                }
            }

            if (!hasChanges)
            {
                MessageBox.Show(
                    "Изменения не внесены",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            foreach (OrderView item in _orders)
            {
                if (!DateTime.TryParse(item.DeliveryDate, out DateTime date))
                {
                    MessageBox.Show(
                        "Дата доставки введена неверно",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }
            }

            foreach (OrderView item in _orders)
            {
                Order order = service.GetById(item.Id);

                order.DeliveryDate = item.DeliveryDate;
                order.StatusId = item.StatusId;

                service.Update(order);
            }

            MessageBox.Show(
                "Изменения успешно сохранены",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Сбрасывает результаты поиска и отображает полный список заказов.
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.ItemsSource == _orders)
            {
                MessageBox.Show(
                    "Список заказов уже отображается полностью.",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            OrderNumberTextBox.Clear();

            OrdersDataGrid.ItemsSource = _orders;

            MessageBox.Show(
                "Фильтр успешно сброшен.",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Закрывает окно заказов.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}