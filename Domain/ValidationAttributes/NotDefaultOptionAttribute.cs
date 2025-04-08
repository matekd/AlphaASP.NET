using System.ComponentModel.DataAnnotations;

namespace Domain.ValidationAttributes;

public class NotDefaultOptionAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false;
        if (value is string)
        {
            Console.WriteLine("string " + value);
            if (string.IsNullOrEmpty((string) value))
                return false;
        }
        else if (value is int)
        {
            Console.WriteLine("int " + value);
            if ((int)value == 0)
                return false;
        }
        return true;
    }
}
