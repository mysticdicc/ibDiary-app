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
            var list = 
                await _dbService.CalendarDays
                    .Include(x => x.MedicineReports)
                    .Include(x => x.MedicineStateChanges)
                    .Include(x => x.SymptomReports)
                    .Include(x => x.SymptomStateChanges)
                    .Include(x => x.CreatedFoods)
                    .Include(x => x.FoodReports)
                    .Include(x => x.CreatedMeals)
                    .Include(x => x.MealReports)
                    .ToListAsync();

            return list;
        }

        public async Task<CalendarDay?> GetByIdAsync(DateOnly date)
        {
            var day =
                await _dbService.CalendarDays
                    .Include(x => x.MedicineReports)
                    .Include(x => x.MedicineStateChanges)
                    .Include(x => x.SymptomReports)
                    .Include(x => x.SymptomStateChanges)
                    .Include(x => x.CreatedFoods)
                    .Include(x => x.FoodReports)
                    .Include(x => x.CreatedMeals)
                    .Include(x => x.MealReports)
                    .FirstOrDefaultAsync(x => x.Date == date);

            return day;
        }

        public async Task<List<CalendarDay>> GetFromDateAsync(DateOnly from)
        {
            return await _dbService.CalendarDays
                .Where(d => d.Date >= from)
                .Include(x => x.MedicineReports)
                .Include(x => x.MedicineStateChanges)
                .Include(x => x.SymptomReports)
                .Include(x => x.SymptomStateChanges)
                .Include(x => x.CreatedFoods)
                .Include(x => x.FoodReports)
                .Include(x => x.CreatedMeals)
                .Include(x => x.MealReports)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(CalendarDay item)
        {
            var dbItem = await GetByIdAsync(item.Date);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(item);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
