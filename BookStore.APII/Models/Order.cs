using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.APII.Models;

[Table("Orders")]
public class Order
{
    [Key]
    public int Id { get; set; }

    public int OrderNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int UserId { get; set; }

    public int PickupCode { get; set; }

    public int StatusId { get; set; }

    public string UserName { get; set; } = "";

    public string StatusName { get; set; } = "";

    public string Articles { get; set; } = "";
}