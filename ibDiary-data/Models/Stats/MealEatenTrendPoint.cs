using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class MealEatenTrendPoint : IUpdatableObject<MealEatenTrendPoint>, IMergableListItem<List<MealEatenTrendPoint>>
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

        public void UpdateProperties(MealEatenTrendPoint source)
        {
            Date = source.Date;
            StartHour = source.StartHour;
            Count = source.Count;
        }

        public void MergeToList(List<MealEatenTrendPoint> target)
        {
            var existing = target.FirstOrDefault(x => x.Date == Date && x.StartHour == StartHour);
            if (null == existing) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}