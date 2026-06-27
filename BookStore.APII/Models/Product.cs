using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.APII.Models;

/// <summary>
/// Представляет информацию о товаре (книге).
/// </summary>
[Table("Products")]
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Article { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Unit { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    public string? Author { get; set; }

    [Required]
    public int ManufacturerId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int Discount { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }
}