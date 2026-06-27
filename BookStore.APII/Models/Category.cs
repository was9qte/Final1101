using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.APII.Models;

[Table("Categories")]
public class Category
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}