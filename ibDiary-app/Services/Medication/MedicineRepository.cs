using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class MedicineRepository : IDatabaseService<Medicine>
    {
        private readonly SQLiteAsyncConnection _dbService;

        public MedicineRepository(SQLiteAsyncConnection connection)
        {
            _dbService = connection;
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            return await _dbService.Table<Medicine>().ToListAsync();
        }

        public async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<Medicine>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(Medicine medicine)
        {
            return await _dbService.UpdateAsync(medicine) > 0;
        }

        public async Task<int> AddAsync(Medicine medicine)
        {
            await _dbService.InsertAsync(medicine);
            return medicine.Id;
        }

        public async Task<bool> DeleteAsync(Medicine medicine)
        {
            return await _dbService.DeleteAsync<Medicine>(medicine.Id) > 0;
        }
    }
}
