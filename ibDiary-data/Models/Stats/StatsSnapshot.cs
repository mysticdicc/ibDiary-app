using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    [Index(nameof(MonthEnd), IsUnique = true)]
    public class StatsSnapshot : IStatsObject<AppDbContext>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly MonthEnd { get; set; }
        [NotMapped]
        public DateOnly MonthBefore { get => MonthEnd.AddMonths(-1); }
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

        public StatsSnapshot(DateOnly monthEnd)
        {
            Id = 0;
            MonthEnd = monthEnd;
            MedicineCount = 0;
            ActiveMedicineCount = 0;
            MedicineStats = [];
            SymptomCount = 0;
            ActiveSymptomCount = 0;
            SymptomStats = [];
            TotalMedicineReports = 0;
            MonthlyMedicalReports = 0;
            MonthlyMedicinesTaken = 0;
            TotalSymptomReports = 0;
            MonthlySymptomReports = 0;
            UniqueMonthlyFoodItems = 0;
            UniqueMonthlyMeals = 0;
            TotalFoodReports = 0;
            MonthlyFoodReports = 0;
            FoodStats = [];
            TotalMealReports = 0;
            MonthlyMealReports = 0;
            MealStats = [];
        }

        public StatsSnapshot()
        {
            Id = 0;
            MonthEnd = DateOnly.FromDateTime(DateTime.UtcNow);
            MedicineCount = 0;
            ActiveMedicineCount = 0;
            MedicineStats = [];
            SymptomCount = 0;
            ActiveSymptomCount = 0;
            SymptomStats = [];
            TotalMedicineReports = 0;
            MonthlyMedicalReports = 0;
            MonthlyMedicinesTaken = 0;
            TotalSymptomReports = 0;
            MonthlySymptomReports = 0;
            UniqueMonthlyFoodItems = 0;
            UniqueMonthlyMeals = 0;
            TotalFoodReports = 0;
            MonthlyFoodReports = 0;
            FoodStats = [];
            TotalMealReports = 0;
            MonthlyMealReports = 0;
            MealStats = [];
        }

        public void UpdateProperties(StatsSnapshot snapshot)
        {
            MonthEnd = snapshot.MonthEnd;
            MedicineCount = snapshot.MedicineCount;
            ActiveMedicineCount = snapshot.ActiveMedicineCount;
            MedicineStats = snapshot.MedicineStats;
            SymptomCount = snapshot.SymptomCount;
            ActiveSymptomCount = snapshot.ActiveSymptomCount;
            SymptomStats = snapshot.SymptomStats;
            TotalMedicineReports = snapshot.TotalMedicineReports;
            MonthlyMedicalReports = snapshot.MonthlyMedicalReports;
            MonthlyMedicinesTaken = snapshot.MonthlyMedicinesTaken;
            TotalSymptomReports = snapshot.TotalSymptomReports;
            MonthlySymptomReports = snapshot.MonthlySymptomReports;
            UniqueMonthlyFoodItems = snapshot.UniqueMonthlyFoodItems;
            UniqueMonthlyMeals = snapshot.UniqueMonthlyMeals;
            TotalFoodReports = snapshot.TotalFoodReports;
            MonthlyFoodReports = snapshot.MonthlyFoodReports;
            FoodStats = snapshot.FoodStats;
            TotalMealReports = snapshot.TotalMealReports;
            MonthlyMealReports = snapshot.MonthlyMealReports;
            MealStats = snapshot.MealStats;
        }

        public async Task GenerateStats(AppDbContext context, DateOnly monthEnd)
        {
            MonthEnd = monthEnd;

            MedicineStats = [];
            SymptomStats = [];
            FoodStats = [];
            MealStats = [];

            var medicines = await context.Medicines
                .Include(x => x.MedicineReports)
                .Include(x => x.StateChanges)
                .Include(x => x.MedicineOccurances)
                .ToListAsync();

            MedicineCount = medicines.Count;
            ActiveMedicineCount = medicines.Count(x => x.Active);
            TotalMedicineReports = medicines.Sum(x => x.MedicineReports.Count);
            MonthlyMedicalReports = medicines.Sum(x =>
                x.MedicineReports.Count(r => r.MedicineTakenAtDate >= MonthBefore && r.MedicineTakenAtDate < MonthEnd));
            MonthlyMedicinesTaken = medicines.Sum(x =>
                x.MedicineReports.Count(r => r.MedicineTaken && r.MedicineTakenAtDate >= MonthBefore && r.MedicineTakenAtDate < MonthEnd));

            foreach (var medicine in medicines)
            {
                var snapshot = new MedicineStatsSnapshot(medicine);
                await snapshot.GenerateStats(medicine, MonthBefore);
                MedicineStats.Add(snapshot);
            }

            var symptoms = await context.Symptoms
                .Include(x => x.SymptomReports)
                .Include(x => x.StateChanges)
                .ToListAsync();

            SymptomCount = symptoms.Count;
            ActiveSymptomCount = symptoms.Count(x => x.Active);
            TotalSymptomReports = symptoms.Sum(x => x.SymptomReports.Count);
            MonthlySymptomReports = symptoms.Sum(x =>
                x.SymptomReports.Count(r => r.SubmittedForDate >= MonthBefore && r.SubmittedForDate < MonthEnd));

            foreach (var symptom in symptoms)
            {
                var snapshot = new SymptomStatsSnapshot(symptom);
                await snapshot.GenerateStats(symptom, MonthBefore);
                SymptomStats.Add(snapshot);
            }

            var foods = await context.FoodItems
                .Include(x => x.FoodReports)
                .ToListAsync();

            TotalFoodReports = foods.Sum(x => x.FoodReports.Count);
            MonthlyFoodReports = foods.Sum(x =>
                x.FoodReports.Count(r => DateOnly.FromDateTime(r.AteFoodAt) >= MonthBefore && DateOnly.FromDateTime(r.AteFoodAt) < MonthEnd));
            UniqueMonthlyFoodItems = foods.Count(x =>
                x.FoodReports.Any(r => DateOnly.FromDateTime(r.AteFoodAt) >= MonthBefore && DateOnly.FromDateTime(r.AteFoodAt) < MonthEnd));

            foreach (var food in foods)
            {
                var snapshot = new FoodStatsSnapshot(food);
                await snapshot.GenerateStats(food, MonthBefore);
                FoodStats.Add(snapshot);
            }

            var meals = await context.Meals
                .Include(x => x.MealReports)
                .ToListAsync();

            TotalMealReports = meals.Sum(x => x.MealReports.Count);
            MonthlyMealReports = meals.Sum(x =>
                x.MealReports.Count(r => DateOnly.FromDateTime(r.AteMealAt) >= MonthBefore && DateOnly.FromDateTime(r.AteMealAt) < MonthEnd));
            UniqueMonthlyMeals = meals.Count(x =>
                x.MealReports.Any(r => DateOnly.FromDateTime(r.AteMealAt) >= MonthBefore && DateOnly.FromDateTime(r.AteMealAt) < MonthEnd));

            foreach (var meal in meals)
            {
                var snapshot = new MealStatsSnapshot(meal);
                await snapshot.GenerateStats(meal, MonthBefore);
                MealStats.Add(snapshot);
            }
        }
    }
}
