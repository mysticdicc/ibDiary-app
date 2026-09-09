using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Food.Dto
{
    public class MealReportDto
    {
        public int Id { get; set; }

        [NotNewCalendarObject(ErrorMessage = "Meal is required.")]
        public MealDto Meal { get; set; }

        public DateTime CreatedAtLocal { get; set; }
        public DateOnly CreatedAtLocalDate { get => DateOnly.FromDateTime(CreatedAtLocal); }

        public DateTime AteMealAtLocal { get; set; }

        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }

        public bool IsNew { get; set; }

        public MealReportDto()
        {
            Id = 0;
            Meal = new();
            CreatedAtLocal = DateTime.Now;
            AteMealAtLocal = CreatedAtLocal;
            Notes = string.Empty;
            IsNew = true;
        }
    }
}
