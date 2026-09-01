using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using ibDiary_app.Services.Calendar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ibDiary_app.Services.Medication;

namespace ibDiary_app.Services
{
    public class MedicineReportRepository : IDatabaseService<MedicineReport>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;
        private readonly MedicineOccuranceRepository _occuranceService;
        public MedicineReportRepository(AppDbContext connection, CalendarDayGenerationService cal, MedicineOccuranceRepository occ)
        {
            _dbService = connection;
            _calendarService = cal;
            _occuranceService = occ;
        }

        public async Task<List<MedicineReport>> GetAllAsync()
        {
            return await _dbService.MedicineReports.Include(x => x.Medicine).ToListAsync();
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
            await HandleDueAtUpdates(medicine);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        private async Task HandleDueAtUpdates(MedicineReport report)
        {
            var dbItem = await _occuranceService.GetByIdAsync(report.DueAt.Id);
            if (dbItem == null) return;

            if (report.MedicineTaken) dbItem.Status = MedicineDueAtStatus.Taken;
            else dbItem.Status = MedicineDueAtStatus.Missed;
        }

        public async Task<int> AddAsync(MedicineReport medicine)
        {
            medicine.IsNew = false;
            await HandleDueAtUpdates(medicine);

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
                .Where(x => x.Medicine == medicine)
                .ToListAsync();

            items = items.Where(x => x.MedicineTakenAtDate == date).ToList();

            return items;
        }
    }
}
