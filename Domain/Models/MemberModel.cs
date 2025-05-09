﻿using Domain.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class MemberModel
{
    public string? Id { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? MemberImage { get; set; }
    public string? ImageUrl { get; set; }

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

    [Display(Name = "Address")]
    public MemberAddress? Address { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }
}
