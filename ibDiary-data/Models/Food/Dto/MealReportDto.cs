using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Food.Dto
{
    public class MealReportDto : ICalendarUpdate
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

        public MealReportDto(MealDto meal)
        {
            Id = 0;
            Meal = meal;
            CreatedAtLocal = DateTime.Now;
            AteMealAtLocal = CreatedAtLocal;
            Notes = string.Empty;
            IsNew = true;
        }

        public DateOnly GetDate() => CreatedAtLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = AteMealAtLocal.Minute.ToString("D2");
            list.Add($"You ate meal {Meal.Name} at {AteMealAtLocal.Hour}:{minute}.");
            return list;
        }
    }
}
