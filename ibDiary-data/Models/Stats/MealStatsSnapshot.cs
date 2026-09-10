using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class MealStatsSnapshot : IUpdatableObject<MealStatsSnapshot>, IMergableListItem<List<MealStatsSnapshot>>
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

        public void UpdateProperties(MealStatsSnapshot source)
        {
            TotalReportsCount = source.TotalReportsCount;
            MonthlyReportsCount = source.MonthlyReportsCount;

            foreach (var item in source.MealEatenByHour)
            {
                item.MergeToList(MealEatenByHour);
            }

            MealEatenByHour.RemoveAll(existing =>
                !source.MealEatenByHour.Any(x => x.Date == existing.Date && x.StartHour == existing.StartHour));
        }

        public void MergeToList(List<MealStatsSnapshot> target)
        {
            var existing = target.FirstOrDefault(x => x.Meal.Id == Meal.Id);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}
