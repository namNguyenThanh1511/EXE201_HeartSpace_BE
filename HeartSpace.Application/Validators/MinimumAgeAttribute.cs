using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Validators
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly dateOfBirth)
            {
                var age = DateTime.Today.Year - dateOfBirth.Year;
                if (dateOfBirth.DayOfYear > DateTime.Today.DayOfYear)
                    age--;

                if (age < _minimumAge)
                {
                    return new ValidationResult(ErrorMessage ?? $"Must be at least {_minimumAge} years old");
                }
            }

            return ValidationResult.Success;
        }
    }
}
