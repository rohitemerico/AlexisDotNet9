﻿using System.ComponentModel.DataAnnotations;

namespace Alexis.Dashboard.Helper;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_extensions.Contains(extension))
            {
                return new ValidationResult($"Invalid file type. Please select {string.Join(", ", _extensions)} file to upload.");
            }
        }
        return ValidationResult.Success;
    }
}
