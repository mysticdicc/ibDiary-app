using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using ibDiary_app.Services.Calendar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class MedicineStateChangeRepository : IDatabaseService<MedicineStateChange>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;

        public MedicineStateChangeRepository(AppDbContext connection, CalendarDayGenerationService cal)
        {
            _dbService = connection;
            _calendarService = cal;
        }

        public async Task<List<MedicineStateChange>> GetAllAsync()
        {
            return await _dbService.MedicineStateChanges.ToListAsync();
        }

        public async Task<MedicineStateChange?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<MedicineStateChange>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(MedicineStateChange medicine)
        {
            throw new NotImplementedException("State changes are not designed to be edited.");
        }

        public async Task<int> AddAsync(MedicineStateChange medicine)
        {
            await _dbService.MedicineStateChanges.AddAsync(medicine);
            await _calendarService.NotifyUpdateCalendarDayAsync(medicine);

            return medicine.Id;
        }

        public async Task<bool> DeleteAsync(MedicineStateChange medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            _dbService.MedicineStateChanges.Remove(medicine);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
