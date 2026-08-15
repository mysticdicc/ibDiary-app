using ibDiary_app.Data;
using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Calendar
{
    public class CalendarDayGenerationService
    {
        private readonly AppDbContext _dbContext;
        private readonly CalendarRepositoryService _repo;

        public CalendarDayGenerationService(AppDbContext context, CalendarRepositoryService repo)
        {
            _dbContext = context;
            _repo = repo;
        }

        public async Task<List<CalendarDay>> GenerateCalendarDaysSinceLastAsync()
        {
            var lastDay = await _dbContext.CalendarDays.OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (lastDay == null)
            {
                var day = new CalendarDay();
                await _repo.AddAsync(day);
            }
            else
            {
                for (DateOnly date = lastDay.Date; date <= today; date = date.AddDays(1))
                {
                    var day = new CalendarDay(date);
                    await _repo.AddAsync(day);
                }
            }

            await UpdateAllCalendarDaysAsync();
            return await _repo.GetAllAsync();
        }

        public async Task UpdateAllCalendarDaysAsync()
        {
            var days = await _repo.GetAllAsync();
            foreach (var day in days)
            {
                day.SymptomReports =  await _dbContext.SymptomReports.Where(x => x.SubmittedForDate == day.Date).ToListAsync();
                day.SymptomStateChanges = await _dbContext.SymptomStateChanges.Where(x => x.ChangedAtDate == day.Date).ToListAsync();
                day.MedicineReports = await _dbContext.MedicineReports.Where(x => x.MedicineTakenAtDate == day.Date).ToListAsync();
                day.MedicineStateChanges = await _dbContext.MedicineStateChanges.Where(x => x.ChangedAtDate == day.Date).ToListAsync();
            }
            await _repo.UpdateOrAddListAsync(days);
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
