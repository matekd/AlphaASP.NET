﻿using System.ComponentModel.DataAnnotations;

namespace Domain.ValidationAttributes;

public class NotDefaultOptionAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false;
        if (value is string)
        {
            if (string.IsNullOrEmpty((string) value))
                return false;
        }
        else if (value is int)
        {
            if ((int)value == 0)
                return false;
        }
        return true;
    }
}
