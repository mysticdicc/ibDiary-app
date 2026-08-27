using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNewCalendarObject : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var memberNames = validationContext.MemberName is not null
                ? new[] { validationContext.MemberName }
                : null;

            if (value is not ICalendarUpdate update)
            {
                return new ValidationResult(ErrorMessage ?? "Please select an item.", memberNames);
            }

            if (update.IsNew)
            {
                return new ValidationResult(ErrorMessage ?? "Please select an item.", memberNames);
            }

            return ValidationResult.Success;
        }
    }
}
