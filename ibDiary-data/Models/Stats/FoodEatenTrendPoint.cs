using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace ibDiary_data.Models.Stats
{
    public class FoodEatenTrendPoint : IStatsObject<FoodItem>, IUpdatableObject<FoodEatenTrendPoint>, IMergableListItem<List<FoodEatenTrendPoint>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public int Count { get; set; }

        public FoodEatenTrendPoint()
        {
            var startDate = DateTime.UtcNow;

            Id = 0;
            Date = DateOnly.FromDateTime(startDate);
            StartHour = TimeOnly.FromDateTime(startDate);
            Count = 0;
        }

        public FoodEatenTrendPoint(DateTime startDate)
        {
            Id = 0;
            Date = DateOnly.FromDateTime(startDate);
            StartHour = TimeOnly.FromDateTime(startDate);
            Count = 0;
        }

        public Task GenerateStats(FoodItem food, DateOnly monthBefore)
        {
            var reports = food.FoodReports;
            var endHour = StartHour.AddHours(1);

            var dateReports = reports.Where(x => DateOnly.FromDateTime(x.AteFoodAt) == Date).ToList();
            var relevent = dateReports.Where(x => 
                            TimeOnly.FromDateTime(x.AteFoodAt) >= StartHour &&
                            TimeOnly.FromDateTime(x.AteFoodAt) <= endHour)
                            .ToList();

            Count = relevent.Count();

            return Task.CompletedTask;
        }

        public void UpdateProperties(FoodEatenTrendPoint source)
        {
            Date = source.Date;
            StartHour = source.StartHour;
            Count = source.Count;
        }

        public void MergeToList(List<FoodEatenTrendPoint> source)
        {
            var existing = source.FirstOrDefault(x => x.Date == Date && x.StartHour == StartHour);
            if (existing == null)
            {
                source.Add(this);
            }
            else
            {
                existing.UpdateProperties(this);
            }
        }
    }
}
