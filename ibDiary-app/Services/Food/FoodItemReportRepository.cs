using ibDiary_app.Data;
using ibDiary_app.Models.Food;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Services.Calendar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Food
{
    public class FoodItemReportRepository : IDatabaseService<FoodItemReport>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;

        public FoodItemReportRepository(AppDbContext connection, CalendarDayGenerationService cal)
        {
            _dbService = connection;
            _calendarService = cal;
        }

        public async Task<List<FoodItemReport>> GetAllAsync()
        {
            return await _dbService.FoodReports.ToListAsync();
        }

        public async Task<FoodItemReport?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<FoodItemReport>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(FoodItemReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (null == dbItem) return false;

            dbItem.UpdateProperties(report);
            var rows = await _dbService.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(FoodItemReport report)
        {
            await _dbService.FoodReports.AddAsync(report);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(report);

            return report.Id;
        }

        public async Task<bool> DeleteAsync(FoodItemReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (null == dbItem) return false;

            _dbService.FoodItemReports.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
