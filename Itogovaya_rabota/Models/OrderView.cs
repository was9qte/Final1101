namespace Itogovaya_rabota.Models
{
    /// <summary>
    /// Модель отображения заказа.
    /// </summary>
    public class OrderView
    {
        public int Id { get; set; }

        public int OrderNumber { get; set; }

        public string Articles { get; set; }

        public string OrderDate { get; set; }

        public string DeliveryDate { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PickupCode { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string ProductName { get; set; }
    }
}