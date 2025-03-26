using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class AddProjectModel
{
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Project name is required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Client name is required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [Display(Name = "End Date")]
    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [Display(Name = "Members", Prompt = "Enter title")]
    public ICollection<int>? Members { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [Required(ErrorMessage = "Budget is required")]
    [DataType(DataType.Currency)]
    public int Budget { get; set; }
}
