using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class StatsSnapshot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        [NotMapped]
        public DateTime MonthBehind { get => CreatedAt.AddMonths(-1); }
        [NotMapped]
        public DateOnly MonthBehindDate { get => DateOnly.FromDateTime(MonthBehind); }
        public int MedicineCount { get; set; }
        public int ActiveMedicineCount { get; set; }
        [NotMapped]
        public int InactiveMedicinesCount { get => MedicineCount - ActiveMedicineCount; }
        public List<MedicineStatsSnapshot> MedicineStats { get; set; }
        public int SymptomCount { get; set; }
        public int ActiveSymptomCount { get; set; }
        [NotMapped]
        public int InactiveSymptomsCount { get => SymptomCount - ActiveSymptomCount; }
        public List<SymptomStatsSnapshot> SymptomStats { get; set; }
        public int TotalMedicineReports { get; set; }
        public int MonthlyMedicalReports { get; set; }
        public int MonthlyMedicinesTaken { get; set; }
        [NotMapped]
        public int MonthlyMedicinesNotTaken { get => MonthlyMedicalReports - MonthlyMedicinesTaken; }
        public int TotalSymptomReports { get; set; }
        public int MonthlySymptomReports { get; set; }
        public int UniqueMonthlyFoodItems { get; set; }
        public int UniqueMonthlyMeals { get; set; }
        public int TotalFoodReports { get; set; }
        public int MonthlyFoodReports { get; set; }
        public List<FoodStatsSnapshot> FoodStats { get; set; }
        public int TotalMealReports { get; set; }
        public int MonthlyMealReports { get; set; }
        public List<MealStatsSnapshot> MealStats { get; set; }
        [NotMapped]
        public int TotalReports { get => TotalMealReports + TotalFoodReports + TotalMedicineReports + TotalSymptomReports; }
        [NotMapped]
        public int TotalMonthlyReports { get => MonthlyMealReports + MonthlyFoodReports + MonthlyMedicalReports + MonthlySymptomReports; }
    }
}
