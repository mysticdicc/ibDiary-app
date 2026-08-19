using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Food
{
    public class FoodItem : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public bool IsNew { get; set; }

        public FoodItem()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            CreatedAt = DateTime.UtcNow;
            IsNew = true;
        }

        public void UpdateProperties(FoodItem food)
        {
            Name = food.Name;
            Description = food.Description;
        }

        public DateOnly GetDate() => CreatedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.CreatedFoods.Contains(this)) day.CreatedFoods.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            list.Add($"Food item {Name} was added.");
            return list;
        }
    }
}
