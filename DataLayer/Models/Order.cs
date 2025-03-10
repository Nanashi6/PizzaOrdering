using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PizzaOrdering.DataLayer.Interfaces;

namespace PizzaOrdering.DataLayer.Models;

public class Order : ITable
{
    [Key]
    public int Id { get; init; }
    [Column]
    public int PizzasCount { get; set; }
    [Column]
    public string Address { get; set; }
    [Column]
    public DateTime OrderDate { get; set; }
    [Column]
    public DateTime DeliveryDate { get; set; }
    [Column]
    public double Price { get; set; }

    [JsonIgnore] public virtual IEnumerable<PizzaRequired>? Pizzas { get; set; }
    
    [Column]
    public string UserId { get; set; }
    [JsonIgnore] 
    public virtual User? User { get; set; }

    // public void Update(CreateOrderDto orderDto)
    // {
    //     OrderDate = orderDto.OrderDate;
    //     DeliveryDate = orderDto.DeliveryDate;
    //     Address = orderDto.Address;
    // }
}