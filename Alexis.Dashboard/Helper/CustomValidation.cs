using System.ComponentModel.DataAnnotations;

namespace Alexis.Dashboard.Helper;

public class CustomValidation
{
}
public class LatitudeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        if (double.TryParse(value.ToString(), out double lat) && lat >= -90 && lat <= 90)
        {
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)lat)[3])[2];
            if (decimalPlaces <= 6)
                return ValidationResult.Success;
        }

        return new ValidationResult("Invalid latitude. Must be between -90 and 90 with up to 6 decimal places.");
    }
}


public class LongitudeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        if (double.TryParse(value.ToString(), out double lng) && lng >= -180 && lng <= 180)
        {
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)lng)[3])[2];
            if (decimalPlaces <= 6)
                return ValidationResult.Success;
        }

        return new ValidationResult("Invalid longitude. Must be between -180 and 180 with up to 6 decimal places.");
    }
}