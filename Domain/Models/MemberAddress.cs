using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class MemberAddress
{
    [Display(Prompt = "Your street name")]
    public string? StreetName { get; set; }
    [Display(Prompt = "Your postal code")]
    public string? PostalCode { get; set; }
    [Display(Prompt = "Your city")]
    public string? City { get; set; }
}

public class MemberAddressDto
{
    public string MemberId { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}