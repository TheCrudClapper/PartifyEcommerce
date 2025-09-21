using System.ComponentModel.DataAnnotations;

namespace CSOS.UI.CustomValidators
{
    public class EmptyListValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var list = value as ICollection<int>;
            if (list != null && list.Count > 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.IsNullOrEmpty(ErrorMessage) ? "You have to select at least one value" : ErrorMessage);
        }
    }
}
