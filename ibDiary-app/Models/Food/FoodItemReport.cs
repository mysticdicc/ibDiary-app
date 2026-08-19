using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Food
{
    public class FoodItemReport
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public FoodItem FoodItem { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public DateTime AteMealAt { get; set; }
        public string Notes { get; set; }

        public FoodItemReport()
        {
            Id = 0;
            FoodItem = new();
            CreatedAt = DateTime.UtcNow;
            AteMealAt = CreatedAt;
            Notes = string.Empty;
        }

        public FoodItemReport(FoodItem item)
        {
            Id = 0;
            FoodItem = item;
            CreatedAt = DateTime.UtcNow;
            AteMealAt = CreatedAt;
            Notes = string.Empty;
        }
    }
}
