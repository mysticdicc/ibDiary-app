using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Interfaces
{
    public interface IStatsObject<T> where T : class
    {
        public Task GenerateStats(T source, DateOnly monthBefore);
    }
}
