using Domain.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ProjectModel
{
    public int Id { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }
    public string? ImageUrl { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Project name is required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Client name is required")]
    public int ClientId { get; set; }

    [Display(Name = "Project status", Prompt = "Status")]
    [Required(ErrorMessage = "A status is required")]
    public int StatusId { get; set; }

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Start date is required")]
    [NotDefaultDate(ErrorMessage = "Select a date")]
    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [Display(Name = "End Date")]
    [Required(ErrorMessage = "End date is required")]
    [NotDefaultDate(ErrorMessage = "Select a date")]
    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [Required(ErrorMessage = "Budget is required")]
    [DataType(DataType.Currency)]
    public int Budget { get; set; }

    public IEnumerable<Member>? Members { get; set; }

    [Display(Name = "Members")]
    public IEnumerable<string>? MemberIds { get; set; }
}