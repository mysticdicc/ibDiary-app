using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Food
{
    public class Meal : ICalendarUpdate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Name must not exceed 128 characters.")]
        public string Name { get; set; }
        public List<FoodItem> FoodItems { get; set; }
        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] 
        public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public List<MealReport> MealReports { get; set; }
        public bool IsNew { get; set; }

        public Meal()
        {
            Id = 0;
            Name = string.Empty;
            FoodItems = [];
            Notes = string.Empty;
            CreatedAt = DateTime.UtcNow;
            IsNew = true;
            MealReports = [];
        }

        public void UpdateProperties(Meal meal)
        {
            Name = meal.Name;
            FoodItems = meal.FoodItems;
            Notes = meal.Notes;
            MealReports = meal.MealReports;
        }

        public DateOnly GetDate() => CreatedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.CreatedMeals.Contains(this)) day.CreatedMeals.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            list.Add($"Meal {Name} was added.");
            return list;
        }

        public Meal Clone()
        {
            var clone = new Meal();

            foreach (var property in typeof(Meal).GetProperties())
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
