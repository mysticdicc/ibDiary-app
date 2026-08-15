using ibDiary_app.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Interfaces
{
    public interface ICalendarUpdate
    {
        public DateOnly GetDate();
        public void AddToCalendarDay(CalendarDay day);
    }
}
