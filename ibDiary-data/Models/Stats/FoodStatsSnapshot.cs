using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class FoodStatsSnapshot : IUpdatableObject<FoodStatsSnapshot>, IMergableListItem<List<FoodStatsSnapshot>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public FoodItem Food { get; set; }
        public int TotalReportsCount { get; set; }
        public int MonthlyReportsCount { get; set; }
        public List<FoodEatenTrendPoint> FoodEatenByHour { get; set; }

        public FoodStatsSnapshot()
        {
            Id = 0;
            Food = new();
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            FoodEatenByHour = [];
        }

        public FoodStatsSnapshot(FoodItem food)
        {
            Id = 0;
            Food = food;
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            FoodEatenByHour = [];
        }

        public void UpdateProperties(FoodStatsSnapshot source)
        {
            TotalReportsCount = source.TotalReportsCount;
            MonthlyReportsCount = source.MonthlyReportsCount;

            foreach (var item in source.FoodEatenByHour)
            {
                item.MergeToList(FoodEatenByHour);
            }

            FoodEatenByHour.RemoveAll(existing =>
                !source.FoodEatenByHour.Any(x => x.Date == existing.Date && x.StartHour == existing.StartHour));
        }

        public void MergeToList(List<FoodStatsSnapshot> target)
        {
            var existing = target.FirstOrDefault(x => x.Food.Id == Food.Id);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}