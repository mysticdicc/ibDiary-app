using ibDiary_data.Models.Food.Dto;
using ibDiary_data.Models.Medication.Dto;
using ibDiary_data.Models.Settings.Dto;
using ibDiary_data.Models.Symptoms.Dto;
using System;
using System.Collections.Generic;

namespace ibDiary_data.Models.Calendar.Dto
{
    public class CalendarDayDto
    {
        public DateOnly Date { get; set; }

        public string DayOfWeek { get => Date.DayOfWeek.ToString(); }
        public string Month { get => Date.ToString("MMMM"); }
        public string Year { get => Date.Year.ToString(); }

        public List<MedicineReportDto> MedicineReports { get; set; }
        public List<MedicineStateChangeDto> MedicineStateChanges { get; set; }
        public List<SymptomReportDto> SymptomReports { get; set; }
        public List<SymptomStateChangeDto> SymptomStateChanges { get; set; }
        public List<MedicineDto> CreatedMedicines { get; set; }
        public List<SymptomDto> CreatedSymptoms { get; set; }
        public List<FoodItemDto> CreatedFoods { get; set; }
        public List<FoodItemReportDto> FoodReports { get; set; }
        public List<MealDto> CreatedMeals { get; set; }
        public List<MealReportDto> MealReports { get; set; }
        public List<ScheduledNotificationDto> CreatedNotifications { get; set; }

        public bool IsNew { get; set; }

        public int ItemCount
        {
            get => MedicineReports.Count + MedicineStateChanges.Count + SymptomReports.Count + SymptomStateChanges.Count +
                   CreatedMedicines.Count + CreatedSymptoms.Count + CreatedFoods.Count + FoodReports.Count + CreatedMeals.Count +
                   MealReports.Count + CreatedNotifications.Count;
        }

        public CalendarDayDto()
        {
            Date = DateOnly.FromDateTime(DateTime.Now);
            MedicineReports = [];
            MedicineStateChanges = [];
            SymptomReports = [];
            SymptomStateChanges = [];
            CreatedMedicines = [];
            CreatedSymptoms = [];
            CreatedFoods = [];
            FoodReports = [];
            CreatedMeals = [];
            MealReports = [];
            CreatedNotifications = [];
            IsNew = true;
        }

        public CalendarDayDto(DateOnly date)
        {
            Date = date;
            MedicineReports = [];
            MedicineStateChanges = [];
            SymptomReports = [];
            SymptomStateChanges = [];
            CreatedMedicines = [];
            CreatedSymptoms = [];
            CreatedFoods = [];
            FoodReports = [];
            CreatedMeals = [];
            MealReports = [];
            CreatedNotifications = [];
            IsNew = true;
        }

        public CalendarDayDto Clone()
        {
            var clone = new CalendarDayDto();

            foreach (var property in typeof(CalendarDayDto).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            return clone;
        }
    }
}