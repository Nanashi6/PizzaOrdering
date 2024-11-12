using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password, ErrorMessage = "Password is not valid")]
    public string Password { get; set; }
}