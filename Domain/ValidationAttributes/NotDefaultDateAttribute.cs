using System.ComponentModel.DataAnnotations;

namespace Domain.ValidationAttributes;

public class NotDefaultDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false;
        if (value is DateOnly)
        {
            if (value.ToString() == "0001-01-01")
                return false;
        }
        return true;
    }
}
