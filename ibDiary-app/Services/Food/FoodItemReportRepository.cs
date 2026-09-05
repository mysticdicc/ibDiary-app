using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Stats;
using ibDiary_data.Data;
using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
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
        private readonly StatsGenerationService _statsGenerator;

        public FoodItemReportRepository(
            AppDbContext connection, 
            CalendarDayGenerationService cal,
            StatsGenerationService stats
            )
        {
            _dbService = connection;
            _calendarService = cal;
            _statsGenerator = stats;
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

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(FoodItemReport report)
        {
            report.IsNew = false;
            await _dbService.FoodReports.AddAsync(report);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(report);
            await _statsGenerator.RequestStatsUpdateAsync();

            return report.Id;
        }

        public async Task<bool> DeleteAsync(FoodItemReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (null == dbItem) return false;

            _dbService.FoodReports.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }
    }
}
