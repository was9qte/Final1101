namespace Itogovaya_rabota.Models
{
    /// <summary>
    /// Хранит информацию о текущем авторизованном пользователе.
    /// </summary>
    public static class CurrentUser
    {
        public static User User { get; set; }
    }
}