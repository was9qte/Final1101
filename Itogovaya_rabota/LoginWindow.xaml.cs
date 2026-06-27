using Itogovaya_rabota.Models;
using Itogovaya_rabota.Services;
using System.Windows;

namespace Itogovaya_rabota
{
    /// <summary>
    /// Окно авторизации пользователя.
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService = new UserService();

        public User CurrentUser { get; private set; }

        /// <summary>
        /// Инициализирует окно авторизации.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Выполняет авторизацию пользователя.
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User user = _userService.Login(LoginTextBox.Text, PasswordBox.Password);

            if (user != null)
            {
                CurrentUser = user;

                DialogResult = true;

                Close();
            }
            else
            {
                MessageBox.Show(
                    "Неверный логин или пароль",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Закрывает окно авторизации.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}