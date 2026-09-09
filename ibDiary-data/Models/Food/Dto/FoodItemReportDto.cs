using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Food.Dto
{
    public class FoodItemReportDto
    {
        public int Id { get; set; }

        [NotNewCalendarObject(ErrorMessage = "Food item is required.")]
        public FoodItemDto FoodItem { get; set; }

        public DateTime CreatedAtLocal { get; set; }
        public DateOnly CreatedAtLocalDate { get => DateOnly.FromDateTime(CreatedAtLocal); }

        public DateTime AteFoodAtLocal { get; set; }

        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }

        public bool IsNew { get; set; }

        public FoodItemReportDto()
        {
            Id = 0;
            FoodItem = new();
            CreatedAtLocal = DateTime.Now;
            AteFoodAtLocal = CreatedAtLocal;
            Notes = string.Empty;
            IsNew = true;
        }
    }
}
