using ibDiary_data.Data;
using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Calendar
{
    public class CalendarRepositoryService
    {
        private readonly AppDbContext _dbService;

        public CalendarRepositoryService(AppDbContext context)
        {
            _dbService = context;
        }

        private IQueryable<CalendarDay> GetCalendarDays()
        {
            return _dbService.Set<CalendarDay>()
                        .Include(x => x.MedicineReports)
                        .Include(x => x.MedicineStateChanges)
                        .Include(x => x.SymptomReports)
                        .Include(x => x.SymptomStateChanges)
                        .Include(x => x.CreatedMedicines)
                        .Include(x => x.CreatedSymptoms)
                        .Include(x => x.CreatedFoods)
                        .Include(x => x.FoodReports)
                        .Include(x => x.CreatedMeals)
                        .Include(x => x.MealReports)
                        .Include(x => x.CreatedNotifications);
        }

        public async Task<bool> AddAsync(CalendarDay item)
        {
            await _dbService.CalendarDays.AddAsync(item);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> UpdateOrAddListAsync(List<CalendarDay> days)
        {
            foreach (var day in days)
            {
                var clone = day.Clone();
                var dbItem = await GetByIdAsync(day.Date);
                if (dbItem == null)
                {
                    await _dbService.CalendarDays.AddAsync(day);
                }
                else
                {
                    if (dbItem.HasChangedState(clone))
                    {
                        dbItem.UpdateProperties(day);
                    }
                }
            }

            var rows = await _dbService.SaveChangesAsync();
            return rows;
        }

        public async Task<bool> DeleteAsync(CalendarDay item)
        {
            var dbItem = await GetByIdAsync(item.Date);
            if (dbItem == null) return false;

            _dbService.CalendarDays.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<List<CalendarDay>> GetAllAsync()
        {
            return await GetCalendarDays().ToListAsync();
        }

        public async Task<CalendarDay?> GetByIdAsync(DateOnly date)
        {
            return await GetCalendarDays().FirstOrDefaultAsync(x => x.Date == date);
        }

        public async Task<List<CalendarDay>> GetFromDateAsync(DateOnly from)
        {
            return await GetCalendarDays().Where(x => x.Date >= from).ToListAsync();
        }

        public async Task<bool> UpdateAsync(CalendarDay item)
        {
            var entry = _dbService.Entry(item);
            if (entry.State != EntityState.Detached)
            {
                var tracked = await _dbService.SaveChangesAsync();
                return tracked > 0;
            }

            var dbItem = await GetByIdAsync(item.Date);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(item);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
