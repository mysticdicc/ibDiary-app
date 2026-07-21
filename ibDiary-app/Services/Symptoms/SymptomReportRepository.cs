using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class SymptomReportRepository : IDatabaseService<SymptomReport>
    {
        private readonly SQLiteAsyncConnection _dbService;

        public SymptomReportRepository(SQLiteAsyncConnection connection)
        {
            _dbService = connection;
        }

        public async Task<List<SymptomReport>> GetAllAsync()
        {
            return await _dbService.Table<SymptomReport>().ToListAsync();
        }

        public async Task<SymptomReport?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<SymptomReport>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(SymptomReport report)
        {
            return await _dbService.UpdateAsync(report) > 0;
        }

        public async Task<int> AddAsync(SymptomReport report)
        {
            await _dbService.InsertAsync(report);
            return report.Id;
        }

        public async Task<bool> DeleteAsync(SymptomReport report)
        {
            return await _dbService.DeleteAsync<SymptomReport>(report.Id) > 0;
        }
    }
}
