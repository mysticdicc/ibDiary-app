using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Food.Dto
{
    public class FoodItemDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Name must not exceed 128 characters.")]
        public string Name { get; set; }

        [MaxLength(1024, ErrorMessage = "Description must not exceed 1024 characters.")]
        public string Description { get; set; }

        public DateTime CreatedAtLocal { get; set; }
        public DateOnly CreatedAtLocalDate { get => DateOnly.FromDateTime(CreatedAtLocal); }

        public List<FoodItemReportDto> FoodReports { get; set; }
        public bool IsNew { get; set; }

        public FoodItemDto()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            CreatedAtLocal = DateTime.Now;
            FoodReports = [];
            IsNew = true;
        }
    }
}