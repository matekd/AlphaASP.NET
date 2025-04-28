using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class JobTitle
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
}

public class JobTitleModel
{
    public int Id { get; set; }

    [Display(Name = "New Job Title", Prompt = "Enter title")]
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = null!;
}