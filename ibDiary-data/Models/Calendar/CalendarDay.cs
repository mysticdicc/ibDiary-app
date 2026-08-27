using ibDiary_data.Models.Food;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Settings;
using ibDiary_data.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Calendar
{
    public class CalendarDay
    {
        [Key] public DateOnly Date { get; set; }
        [NotMapped] public string DayOfWeek { get => Date.DayOfWeek.ToString(); }
        [NotMapped] public string Month { get => Date.ToString("MMMM");  }
        [NotMapped] public string Year { get => Date.Year.ToString(); }
        public List<MedicineReport> MedicineReports { get; set; }
        public List<MedicineStateChange> MedicineStateChanges { get; set; }
        public List<SymptomReport> SymptomReports { get; set; }
        public List<SymptomStateChange> SymptomStateChanges { get; set; }
        public List<Medicine> CreatedMedicines { get; set; }
        public List<Symptom> CreatedSymptoms { get; set; }
        public List<FoodItem> CreatedFoods { get; set; }
        public List<FoodItemReport> FoodReports { get; set; }
        public List<Meal> CreatedMeals { get; set; }
        public List<MealReport> MealReports { get; set; }
        public List<ScheduledNotification> CreatedNotifications { get; set; }
        public bool IsNew { get; set; }

        public CalendarDay()
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
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

        public CalendarDay(DateOnly date)
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

        public void UpdateProperties(CalendarDay day)
        {
            if (day.Date != Date) return;
            MedicineReports = day.MedicineReports;
            MedicineStateChanges = day.MedicineStateChanges;
            SymptomReports = day.SymptomReports;
            SymptomStateChanges = day.SymptomStateChanges;
            CreatedSymptoms = day.CreatedSymptoms;
            CreatedMedicines = day.CreatedMedicines;
            CreatedFoods = day.CreatedFoods;
            FoodReports = day.FoodReports;
            CreatedMeals = day.CreatedMeals;
            MealReports = day.MealReports;
            CreatedNotifications = day.CreatedNotifications;
        }

        public bool HasChangedState(CalendarDay old)
        {
            return
                MedicineReports != old.MedicineReports ||
                MedicineStateChanges != old.MedicineStateChanges ||
                SymptomReports != old.SymptomReports ||
                SymptomStateChanges != old.SymptomStateChanges ||
                CreatedMedicines != old.CreatedMedicines ||
                CreatedSymptoms != old.CreatedSymptoms ||
                CreatedFoods != old.CreatedFoods ||
                FoodReports != old.FoodReports ||
                CreatedMeals != old.CreatedMeals ||
                MealReports != old.MealReports ||
                CreatedNotifications != old.CreatedNotifications;
        }

        public CalendarDay Clone()
        {
            var clone = new CalendarDay();

            foreach (var property in typeof(CalendarDay).GetProperties())
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
