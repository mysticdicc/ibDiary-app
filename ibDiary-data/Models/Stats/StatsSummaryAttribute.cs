using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class StatsSummaryAttribute : Attribute
    {
        public string IncreaseText { get; set; }
        public string DecreaseText { get; set; }

        public StatsSummaryAttribute(
            string increase, string decrease)
        {
            IncreaseText = increase;
            DecreaseText = decrease;
        }
    }
}