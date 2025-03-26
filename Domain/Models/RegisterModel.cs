using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class RegisterModel
{
    [Display(Name = "First name", Prompt = "Your first name")]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = "Your last name")]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;

    //[RegularExpression("")]
    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    //[RegularExpression("")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [Required(ErrorMessage = "Field is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;
    
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions")]
    public bool Terms { get; set; }
}
