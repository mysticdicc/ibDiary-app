using ibDiary_data.Data;
using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Calendar
{
    public class CalendarDayGenerationService
    {
        private readonly CalendarRepositoryService _repo;

        public CalendarDayGenerationService(CalendarRepositoryService repo)
        {
            _repo = repo;
        }

        public async Task NotifyUpdateCalendarDayAsync(ICalendarUpdate update)
        {
            var date = update.GetDate();
            var day = await _repo.GetByIdAsync(date);

            if (null == day)
            {
                day = new(date);
                update.AddToCalendarDay(day);
                await _repo.AddAsync(day);
            }
            else
            {
                update.AddToCalendarDay(day);
                await _repo.UpdateAsync(day);
            }
        }
    }
}
