using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Stats;
using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class SymptomReportRepository : IDatabaseService<SymptomReport>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;
        private readonly StatsGenerationService _statsGenerator;

        public SymptomReportRepository(
            AppDbContext connection, 
            CalendarDayGenerationService cal,
            StatsGenerationService stats
            )
        {
            _dbService = connection;
            _calendarService = cal;
            _statsGenerator = stats;
        }

        public async Task<List<SymptomReport>> GetAllAsync()
        {
            return await _dbService.SymptomReports.ToListAsync();
        }

        public async Task<SymptomReport?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<SymptomReport>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(SymptomReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (null == dbItem) return false;

            dbItem.UpdateProperties(report);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(SymptomReport report)
        {
            report.IsNew = false;
            await _dbService.SymptomReports.AddAsync(report);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(report);
            await _statsGenerator.RequestStatsUpdateAsync();

            return report.Id;
        }

        public async Task<bool> DeleteAsync(SymptomReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (null == dbItem) return false;

            _dbService.SymptomReports.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }
    }
}
