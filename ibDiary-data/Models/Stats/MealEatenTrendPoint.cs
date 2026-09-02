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

        public MealEatenTrendPoint(DateTime startDate)
        {
            Id = 0;
            Date = DateOnly.FromDateTime(startDate);
            StartHour = TimeOnly.FromDateTime(startDate);
            Count = 0;
        }

        public void GenerateStats(Meal meal, DateTime monthBefore)
        {
            var reports = meal.MealReports;
            var endHour = StartHour.AddHours(1);

            var dateReports = reports.Where(x => DateOnly.FromDateTime(x.AteMealAt) == Date).ToList();
            var relevent = dateReports.Where(x =>
                            TimeOnly.FromDateTime(x.CreatedAt) >= StartHour &&
                            TimeOnly.FromDateTime(x.CreatedAt) <= endHour)
                            .ToList();

            Count = relevent.Count();
        }
    }
}
}
