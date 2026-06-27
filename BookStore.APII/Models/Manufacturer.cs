using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.APII.Models;

/// <summary>
/// Представляет информацию о производителе.
/// </summary>
[Table("Manufacturers")]
public class Manufacturer
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}