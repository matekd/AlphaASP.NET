using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class LoginModel
{
    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool RememberMe {  get; set; } = false;
}
