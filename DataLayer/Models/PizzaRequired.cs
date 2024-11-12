using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PizzaOrdering.DataLayer.Interfaces;

namespace PizzaOrdering.DataLayer.Models;

public class PizzaRequired : ITable
{
    [Key]
    public int Id { get; init; }
    [Column]
    public int PizzaId { get; set; }
    [JsonIgnore]
    public virtual Pizza? Pizza { get; set; }
    [Column]
    public int OrderId { get; set; }
    [JsonIgnore]
    public virtual Order? Order { get; set; }
}