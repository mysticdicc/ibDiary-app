using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Interfaces
{
    public interface IStatsObject<T> where T : class
    {
        public void GenerateStats(T source, DateTime monthBefore);
    }
}
