using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class FoodEatenTrendPoint : IStatsObject<FoodItem>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public int Count { get; set; }

        public FoodEatenTrendPoint(DateTime startDate)
        {
            Id = 0;
            Date = DateOnly.FromDateTime(startDate);
            StartHour = TimeOnly.FromDateTime(startDate);
            Count = 0;
        }

        public void GenerateStats(FoodItem food, DateTime monthBefore)
        {
            var reports = food.FoodReports;
            var endHour = StartHour.AddHours(1);

            var dateReports = reports.Where(x => DateOnly.FromDateTime(x.AteFoodAt) == Date).ToList();
            var relevent = dateReports.Where(x => 
                            TimeOnly.FromDateTime(x.CreatedAt) >= StartHour &&
                            TimeOnly.FromDateTime(x.CreatedAt) <= endHour)
                            .ToList();

            Count = relevent.Count();
        }
    }
}
