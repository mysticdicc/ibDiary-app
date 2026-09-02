using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class MealStatsSnapshot : IStatsObject<Meal>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Meal Meal { get; set; }
        public int TotalReportsCount { get; set; }
        public int MonthlyReportsCount { get; set; }
        public List<MealEatenTrendPoint> MealEatenByHour { get; set; }

        public MealStatsSnapshot()
        {
            Id = 0;
            Meal = new();
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            MealEatenByHour = [];
        }

        public MealStatsSnapshot(Meal meal)
        {
            Id = 0;
            Meal = meal;
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            MealEatenByHour = [];
        } 

        public Task GenerateStats(Meal meal, DateOnly monthBefore)
        {
            var endDate = monthBefore.AddMonths(1);
            var reports = meal.MealReports;

            TotalReportsCount = reports.Count;
            var monthly = reports
                                .Where(x => 
                                DateOnly.FromDateTime(x.AteMealAt) >= monthBefore && 
                                DateOnly.FromDateTime(x.AteMealAt) < endDate)
                                .ToList();

            MonthlyReportsCount = monthly.Count;

            MealEatenByHour = [];
            for (var date = monthBefore; date <= endDate; date = date.AddDays(1))
            {
                for (int i = 1; i < 24; i++)
                {
                    var target = new DateTime(date.Year, date.Month, date.Day, i, 0, 0);
                    var point = new MealEatenTrendPoint(target);
                    point.GenerateStats(meal, monthBefore);
                    MealEatenByHour.Add(point);
                }
            }

            return Task.CompletedTask;
        }
    }
}
