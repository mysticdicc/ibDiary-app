using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Food
{
    public class Meal : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public string Name { get; set; }
        public List<FoodItem> FoodItems { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public bool IsNew { get; set; }

        public Meal()
        {
            Id = 0;
            Name = string.Empty;
            FoodItems = [];
            Notes = string.Empty;
            CreatedAt = DateTime.UtcNow;
            IsNew = true;
        }

        public void UpdateProperties(Meal meal)
        {
            Name = meal.Name;
            FoodItems = meal.FoodItems;
            Notes = meal.Notes;
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
    }
}
