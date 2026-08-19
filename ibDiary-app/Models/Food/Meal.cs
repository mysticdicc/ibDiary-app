using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Food
{
    public class Meal
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
    }
}
