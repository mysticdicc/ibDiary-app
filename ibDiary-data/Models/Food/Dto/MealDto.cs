using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Food.Dto
{
    public class MealDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Name must not exceed 128 characters.")]
        public string Name { get; set; }

        public List<FoodItemDto> FoodItems { get; set; }

        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }

        public DateTime CreatedAtLocal { get; set; }
        public DateOnly CreatedAtLocalDate { get => DateOnly.FromDateTime(CreatedAtLocal); }

        public List<MealReportDto> MealReports { get; set; }
        public bool IsNew { get; set; }

        public MealDto()
        {
            Id = 0;
            Name = string.Empty;
            FoodItems = [];
            Notes = string.Empty;
            CreatedAtLocal = DateTime.Now;
            IsNew = true;
            MealReports = [];
        }
    }
}
