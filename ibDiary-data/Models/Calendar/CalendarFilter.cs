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
        private bool _medicineFilterEnabled;
        public bool MedicineFilterEnabled
        {
            get => _medicineFilterEnabled;
            set
            {
                _medicineFilterEnabled = value;
                if (!value) Medicine = new();
                UpdateShowFilters();
            }
        }
        public Medicine Medicine { get; set; }
        private bool _symptomFilterEnabled;
        public bool SymptomFilterEnabled
        {
            get => _symptomFilterEnabled;
            set
            {
                _symptomFilterEnabled = value;
                if (!value) Symptom = new();
                UpdateShowFilters();
            }
        }
        public Symptom Symptom { get; set; }
        private bool _foodFilterEnabled;
        public bool FoodFilterEnabled
        {
            get => _foodFilterEnabled;
            set
            {
                _foodFilterEnabled = value;
                if (!value) Food = new();
                UpdateShowFilters();
            }
        }
        public FoodItem Food { get; set; }
        private bool _mealFilterEnabled;
        public bool MealFilterEnabled
        {
            get => _mealFilterEnabled;
            set
            {
                _mealFilterEnabled = value;
                if (!value) Meal = new();
                UpdateShowFilters();
            }
        }
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
    }
}
