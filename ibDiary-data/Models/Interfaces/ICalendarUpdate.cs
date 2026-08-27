using ibDiary_data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Interfaces
{
    public interface ICalendarUpdate
    {
        public DateOnly GetDate();
        public void AddToCalendarDay(CalendarDay day);
        public List<string> GetCalendarUpdate();
    }
}
