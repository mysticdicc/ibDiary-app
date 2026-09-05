using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Stats;
using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomStateChangeRepository : IDatabaseService<SymptomStateChange>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;
        private readonly StatsGenerationService _statsGenerator;

        public SymptomStateChangeRepository(
            AppDbContext connection, 
            CalendarDayGenerationService cal,
            StatsGenerationService stats
            )
        {
            _dbService = connection;
            _calendarService = cal;
            _statsGenerator = stats;
        }

        public async Task<List<SymptomStateChange>> GetAllAsync()
        {
            return await _dbService.SymptomStateChanges.ToListAsync();
        }

        public async Task<SymptomStateChange?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<SymptomStateChange>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(SymptomStateChange symptom)
        {
            throw new NotImplementedException("State changes are not designed to be edited.");
        }

        public async Task<int> AddAsync(SymptomStateChange symptom)
        {
            symptom.IsNew = false;

            await _dbService.SymptomStateChanges.AddAsync(symptom);
            await _calendarService.NotifyUpdateCalendarDayAsync(symptom);

            await _statsGenerator.RequestStatsUpdateAsync();

            return symptom.Id;
        }

        public async Task<bool> DeleteAsync(SymptomStateChange symptom)
        {
            var dbItem = await GetByIdAsync(symptom.Id);
            if (dbItem == null) return false;

            _dbService.SymptomStateChanges.Remove(symptom);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }
    }
}
