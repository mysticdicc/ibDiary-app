using ibDiary_data.Models.Food;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Calendar
{
    public class CalendarFilter
    {
        public bool DateFilterEnabled { get; set; }
        public DateOnly DateFilterFrom { get; set; }
        public DateOnly DateFilterTo { get; set; }
        public bool MedicineFilterEnabled { get; set; }
        public Medicine Medicine { get; set; }
        public bool SymptomFilterEnabled { get; set; }
        public Symptom Symptom { get; set; }
        public bool FoodFilterEnabled { get; set; }
        public FoodItem Food { get; set; }
        public bool MealFilterEnabled { get; set; }
        public Meal Meal { get; set; }
        public bool ShowInactive { get; set; }
        public bool ShowMedicineReports { get; set; }
        public bool ShowSymptomReports { get; set; }
        public bool ShowFoodReports { get; set; }
        public bool ShowMealReports { get; set; }
        public bool ShowAddedMedicines { get; set; }
        public bool ShowAddedSymptoms { get; set; }
        public bool ShowAddedFoods { get; set; }
        public bool ShowAddedMeals { get; set; }

        public CalendarFilter()
        {
            DateFilterEnabled = false;
            DateFilterFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-365));
            DateFilterTo = DateOnly.FromDateTime(DateTime.UtcNow);
            MedicineFilterEnabled = false;
            Medicine = new();
            SymptomFilterEnabled = false;
            Symptom = new();
            FoodFilterEnabled = false;
            Food = new();
            MealFilterEnabled = false;
            Meal = new();
            ShowInactive = true;
            ShowMedicineReports = true;
            ShowSymptomReports = true;
            ShowFoodReports = true;
            ShowMealReports = true;
            ShowAddedMedicines = true;
            ShowAddedSymptoms = true;
            ShowAddedFoods = true;
            ShowAddedMeals = true;
        }

        public void UpdateShowFilters()
        {
            if (!MedicineFilterEnabled && !SymptomFilterEnabled && !FoodFilterEnabled && !MealFilterEnabled)
            {
                ShowInactive = true;
                ShowMedicineReports = true;
                ShowSymptomReports = true;
                ShowAddedMedicines = true;
                ShowAddedSymptoms = true;
                ShowAddedFoods = true;
                ShowFoodReports = true;
                ShowAddedMeals = true;
                ShowMealReports = true;
                return;
            }

            ShowInactive = false;
            ShowMedicineReports = false;
            ShowSymptomReports = false;
            ShowAddedMedicines = false;
            ShowAddedSymptoms = false;
            ShowAddedFoods = false;
            ShowFoodReports = false;
            ShowAddedMeals = false;
            ShowMealReports = false;

            if (MedicineFilterEnabled)
            {
                ShowMedicineReports = true;
                ShowAddedMedicines = true;
            }
            if (SymptomFilterEnabled)
            {
                ShowSymptomReports = true;
                ShowAddedSymptoms = true;
            }

            if (FoodFilterEnabled)
            {
                ShowFoodReports = true;
                ShowMealReports = true;
                ShowAddedFoods = true;
                ShowAddedMeals = true; //for meals containing food
            }

            if (MealFilterEnabled)
            {
                ShowMealReports = true;
                ShowAddedMeals = true;
            }
        }

        public void ChangeSymptomFilter(bool enabled)
        {
            if (enabled)
            {
                SymptomFilterEnabled = true;
            }
            else
            {
                SymptomFilterEnabled = false;
                Symptom = new();
            }

            UpdateShowFilters();
        }

        public void ChangeMedicineFilter(bool enabled)
        {
            if (enabled)
            {
                MedicineFilterEnabled = true;
            }
            else
            {
                MedicineFilterEnabled = false;
                Medicine = new();
            }

            UpdateShowFilters();
        }

        public void ChangeFoodFilter(bool enabled)
        {
            if (enabled)
            {
                FoodFilterEnabled = true;
            }
            else
            {
                FoodFilterEnabled = false;
                Food = new();
            }

            UpdateShowFilters();
        }

        public void ChangeMealFilter(bool enabled)
        {
            if (enabled)
            {
                MealFilterEnabled = true;
            }
            else
            {
                MealFilterEnabled = false;
                Meal = new();
            }

            UpdateShowFilters();
        }
    }
}
