using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.Infrastracture.Attributes;

public class CheckDateAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public CheckDateAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
        if (startDateProperty == null)
        {
            return new ValidationResult($"Неизвестное значение: {_startDatePropertyName}");
        }

        var startDateValue = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);
        startDateValue = startDateValue.Value.AddMinutes(30);
        var endDateValue = (DateTime?)value;

        if (endDateValue < startDateValue)
        {
            return new ValidationResult($"Дата не может быть меньше {startDateValue?.ToString("dd.MM.yyyy")}");
        }

        return ValidationResult.Success;
    }
}