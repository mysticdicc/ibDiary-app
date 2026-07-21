using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class MedicineReportRepository : IDatabaseService<MedicineReport>
    {
        private readonly SQLiteAsyncConnection _dbService;

        public MedicineReportRepository(SQLiteAsyncConnection connection)
        {
            _dbService = connection;
        }

        public async Task<List<MedicineReport>> GetAllAsync()
        {
            return await _dbService.Table<MedicineReport>().ToListAsync();
        }

        public async Task<MedicineReport?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<MedicineReport>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(MedicineReport medicine)
        {
            return await _dbService.UpdateAsync(medicine) > 0;
        }

        public async Task<int> AddAsync(MedicineReport medicine)
        {
            await _dbService.InsertAsync(medicine);
            return medicine.Id;
        }

        public async Task<bool> DeleteAsync(MedicineReport medicine)
        {
            return await _dbService.DeleteAsync<MedicineReport>(medicine.Id) > 0;
        }
    }
}
