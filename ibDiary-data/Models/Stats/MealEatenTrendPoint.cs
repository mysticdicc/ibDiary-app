using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class MealEatenTrendPoint : IStatsObject<Meal>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public int Count { get; set; }

        public MealEatenTrendPoint()
        {
            var startDate = DateTime.UtcNow;
            Id = 0;
            Date = DateOnly.FromDateTime(startDate);
            StartHour = TimeOnly.FromDateTime(startDate);
            Count = 0;
        }

        public MealEatenTrendPoint(DateTime startDate)
        {
            Id = 0;
            Date = DateOnly.FromDateTime(startDate);
            StartHour = TimeOnly.FromDateTime(startDate);
            Count = 0;
        }

        public Task GenerateStats(Meal meal, DateOnly monthBefore)
        {
            var reports = meal.MealReports;
            var endHour = StartHour.AddHours(1);

            var dateReports = reports.Where(x => DateOnly.FromDateTime(x.AteMealAt) == Date).ToList();
            var relevent = dateReports.Where(x =>
                            TimeOnly.FromDateTime(x.AteMealAt) >= StartHour &&
                            TimeOnly.FromDateTime(x.AteMealAt) <= endHour)
                            .ToList();

            Count = relevent.Count();

            return Task.CompletedTask;
        }
    }
}