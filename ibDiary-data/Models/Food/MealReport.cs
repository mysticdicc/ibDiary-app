using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Food
{
    public class MealReport : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [NotNewCalendarObject(ErrorMessage = "Meal is required.")]
        public Meal Meal { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public DateTime AteMealAt { get; set; }
        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }
        public bool IsNew { get; set; }

        public MealReport()
        {
            Id = 0;
            Meal = new();
            CreatedAt = DateTime.UtcNow;
            AteMealAt = CreatedAt;
            Notes = string.Empty;
            IsNew = true;
        }

        public MealReport(Meal meal)
        {
            Id = 0;
            Meal = meal;
            CreatedAt = DateTime.UtcNow;
            AteMealAt = CreatedAt;
            Notes = string.Empty;
            IsNew = true;
        }

        public void UpdateProperties(MealReport report)
        {
            Meal = report.Meal;
            AteMealAt = report.AteMealAt;
            Notes = report.Notes;
        }

        public DateOnly GetDate() => CreatedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.MealReports.Contains(this)) day.MealReports.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = AteMealAt.Minute.ToString("D2");
            list.Add($"You ate meal {Meal.Name} at {AteMealAt.Hour}:{minute}.");
            return list;
        }

        public MealReport Clone()
        {
            var clone = new MealReport();

            foreach (var property in typeof(MealReport).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            return clone;
        }
    }
}
