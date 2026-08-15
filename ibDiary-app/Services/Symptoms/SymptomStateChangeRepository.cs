using ibDiary_app.Data;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services.Calendar;
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

        public SymptomStateChangeRepository(AppDbContext connection, CalendarDayGenerationService cal)
        {
            _dbService = connection;
            _calendarService = cal;
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
            await _dbService.SymptomStateChanges.AddAsync(symptom);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(symptom);

            return symptom.Id;
        }

        public async Task<bool> DeleteAsync(SymptomStateChange symptom)
        {
            var dbItem = await GetByIdAsync(symptom.Id);
            if (dbItem == null) return false;

            _dbService.SymptomStateChanges.Remove(symptom);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
