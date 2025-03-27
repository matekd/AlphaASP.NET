namespace Domain.Models;

public class User
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber {  get; set; }
    public DateOnly? BirthDate { get; set; }
    //public File ProfileImage 
    public UserAddress? Address { get; set; } = new();
}
