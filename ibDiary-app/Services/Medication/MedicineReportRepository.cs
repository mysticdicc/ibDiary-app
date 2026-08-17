using ibDiary_app.Data;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services.Calendar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class MedicineReportRepository : IDatabaseService<MedicineReport>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;
        public MedicineReportRepository(AppDbContext connection, CalendarDayGenerationService cal)
        {
            _dbService = connection;
            _calendarService = cal;
        }

        public async Task<List<MedicineReport>> GetAllAsync()
        {
            return await _dbService.MedicineReports.ToListAsync();
        }

        public async Task<MedicineReport?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<MedicineReport>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(MedicineReport medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(medicine);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(MedicineReport medicine)
        {
            medicine.IsNew = false;
            await _dbService.MedicineReports.AddAsync(medicine);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(medicine);

            return medicine.Id;
        }

        public async Task<bool> DeleteAsync(MedicineReport medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            _dbService.MedicineReports.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<List<MedicineReport>> GetReportsForMedicineOnDateAsync(Medicine medicine, DateOnly date)
        {
            var items = 
                await _dbService.MedicineReports
                .Where(x => x.Medicine == medicine && x.MedicineTakenAtDate == date)
                .ToListAsync();

            return items;
        }
    }
}
