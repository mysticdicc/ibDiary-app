using ibDiary_app.Data;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class SymptomReportRepository : IDatabaseService<SymptomReport>
    {
        private readonly AppDbContext _dbService;

        public SymptomReportRepository(AppDbContext connection)
        {
            _dbService = connection;
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

            dbItem = report;
            var rows = await _dbService.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(SymptomReport report)
        {
            report.IsNew = false;
            await _dbService.SymptomReports.AddAsync(report);
            await _dbService.SaveChangesAsync();
            return report.Id;
        }

        public async Task<bool> DeleteAsync(SymptomReport report)
        {
            var dbItem = await GetByIdAsync(report.Id);
            if (null == dbItem) return false;

            _dbService.SymptomReports.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
