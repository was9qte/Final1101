namespace Itogovaya_rabota.Models
{
    /// <summary>
    /// Модель товара.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Article { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public decimal Price { get; set; }

        public string Author { get; set; }

        public int ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        public int CategoryId { get; set; }

        public int Discount { get; set; }

        public int QuantityInStock { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }
}