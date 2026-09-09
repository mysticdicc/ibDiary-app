using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Food.Dto
{
    public class FoodItemReportDto : ICalendarUpdate
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

        public FoodItemReportDto(FoodItemDto food)
        {
            Id = 0;
            FoodItem = food;
            CreatedAtLocal = DateTime.Now;
            AteFoodAtLocal = CreatedAtLocal;
            Notes = string.Empty;
            IsNew = true;
        }

        public DateOnly GetDate() => CreatedAtLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = AteFoodAtLocal.Minute.ToString("D2");
            list.Add($"You ate food item {FoodItem.Name} at {AteFoodAtLocal.Hour}:{minute}.");
            return list;
        }
    }
}
