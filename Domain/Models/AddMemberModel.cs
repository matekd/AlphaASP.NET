using Domain.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class AddMemberModel
{
    [DataType(DataType.Upload)]
    public IFormFile? MemberImage { get; set; }

    [Display(Name = "First name", Prompt = "Your first name")]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = "Your last name")]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;

    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Your phone number")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Must be a valid phone number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Job Title", Prompt = "Your job title")]
    [NotDefaultOption(ErrorMessage = "Member requires a title")]
    public int JobTitleId { get; set; }

    // is split
    public MemberAddress? Address { get; set; }

    // split into day, month, year? try to combine in service, send date or return submission error
    public DateOnly? BirthDate { get; set; }
}
