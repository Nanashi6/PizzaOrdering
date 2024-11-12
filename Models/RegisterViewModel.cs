using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.Models;

public class RegisterViewModel
{    
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password, ErrorMessage = "Invalid Password")]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}