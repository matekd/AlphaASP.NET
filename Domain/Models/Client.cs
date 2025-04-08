using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
public class AddClientModel
{
    [Display(Name = "New Client", Prompt = "Enter client name")]
    [Required(ErrorMessage = "Client name is required")]
    public string Name { get; set; } = null!;
}

public class EditClientModel
{
    [Required]
    public int Id { get; set; }

    [Display(Name = "Client", Prompt = "Enter client name")]
    [Required(ErrorMessage = "Client name is required")]
    public string Name { get; set; } = null!;
}