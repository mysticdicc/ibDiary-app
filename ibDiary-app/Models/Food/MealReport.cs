using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Food
{
    public class MealReport
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public Meal Meal { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public DateTime AteMealAt { get; set; }
        public string Notes { get; set; }

        public MealReport()
        {
            Id = 0;
            Meal = new();
            CreatedAt = DateTime.UtcNow;
            AteMealAt = CreatedAt;
            Notes = string.Empty;
        }

        public MealReport(Meal meal)
        {
            Id = 0;
            Meal = meal;
            CreatedAt = DateTime.UtcNow;
            AteMealAt = CreatedAt;
            Notes = string.Empty;
        }
    }
}
