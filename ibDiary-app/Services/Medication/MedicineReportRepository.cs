using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Medication;
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
    public class MedicineReportRepository : IDatabaseService<MedicineReport>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;
        private readonly StatsGenerationService _statsGenerator;
        public MedicineReportRepository(
            AppDbContext connection, 
            CalendarDayGenerationService cal, 
            StatsGenerationService stats
            )
        {
            _dbService = connection;
            _calendarService = cal;
            _statsGenerator = stats;
        }

        public async Task<List<MedicineReport>> GetAllAsync()
        {
            return await _dbService.MedicineReports.Include(x => x.Medicine).ToListAsync();
        }

        public async Task<MedicineReport?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<MedicineReport>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(MedicineReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(report);
            await HandleDueAtUpdates(report);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }

        private async Task HandleDueAtUpdates(MedicineReport report)
        {
            var dbItem = await _dbService.MedicineOccurances.FindAsync(report.DueAt.Id);

            if (dbItem == null)
            {
                if (report.MedicineTaken) report.DueAt.Status = MedicineDueAtStatus.Taken;
                else report.DueAt.Status = MedicineDueAtStatus.Missed;
                return;
            }

            if (report.MedicineTaken) dbItem.Status = MedicineDueAtStatus.Taken;
            else dbItem.Status = MedicineDueAtStatus.Missed;
            report.DueAt = dbItem;
        }

        public async Task<int> AddAsync(MedicineReport report)
        {
            report.IsNew = false;
            var medicine = await _dbService.Medicines.FindAsync(report.Medicine.Id);
            if (null != medicine)
            {
                report.Medicine = medicine;
                report.MedicineId = medicine.Id;
                report.DueAt.Medicine = medicine;
            }

            await HandleDueAtUpdates(report);

            await _dbService.MedicineReports.AddAsync(report);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(report);
            await _statsGenerator.RequestStatsUpdateAsync();

            return report.Id;
        }

        public async Task<bool> DeleteAsync(MedicineReport medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            _dbService.MedicineReports.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

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
