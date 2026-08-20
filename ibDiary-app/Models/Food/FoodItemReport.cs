using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Food
{
    public class FoodItemReport : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public FoodItem FoodItem { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public DateTime AteFoodAt { get; set; }
        public string Notes { get; set; }
        public bool IsNew { get; set; }

        public FoodItemReport()
        {
            Id = 0;
            FoodItem = new();
            CreatedAt = DateTime.UtcNow;
            AteFoodAt = CreatedAt;
            Notes = string.Empty;
            IsNew = true;
        }

        public FoodItemReport(FoodItem item)
        {
            Id = 0;
            FoodItem = item;
            CreatedAt = DateTime.UtcNow;
            AteFoodAt = CreatedAt;
            Notes = string.Empty;
            IsNew = true;
        }

        public void UpdateProperties(FoodItemReport report)
        {
            FoodItem = report.FoodItem;
            AteFoodAt = report.AteFoodAt;
            Notes = report.Notes;
        }

        public DateOnly GetDate() => CreatedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.FoodReports.Contains(this)) day.FoodReports.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = AteFoodAt.Minute.ToString("D2");
            list.Add($"You ate food item {FoodItem.Name} at {AteFoodAt.Hour}:{minute}.");
            return list;
        }
    }
}
