using ibDiary_app.Services.System;
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
        private readonly ClientNotificationService _notifier;

        public CalendarDayGenerationService(CalendarRepositoryService repo, ClientNotificationService notifier)
        {
            _repo = repo;
            _notifier = notifier;
        }

        public async Task NotifyUpdateCalendarDayAsync(ICalendarUpdate update)
        {
            try
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
            catch (Exception ex)
            {
                _notifier.ShowNotification("Calendar Service Error", ex.Message);
            }
        }
    }
}
