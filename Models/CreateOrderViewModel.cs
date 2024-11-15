using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PizzaOrdering.Infrastracture.Attributes;

namespace PizzaOrdering.Models;

public class CreateOrderViewModel
{
    [Required]
    public int Id { get; init; }
    [Required]
    public string Address { get; set; }
    [Required]
    [CheckDate("CurrentDate")]
    public DateTime DeliveryDate { get; set; }
    public DateTime CurrentDate { get { return DateTime.Now; } }
}
