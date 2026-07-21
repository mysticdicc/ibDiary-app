using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class SymptomRepository : IDatabaseService<Symptom>
    {
        private readonly SQLiteAsyncConnection _dbService;

        public SymptomRepository(SQLiteAsyncConnection connection)
        {
            _dbService = connection;
        }

        public async Task<List<Symptom>> GetAllAsync()
        {
            return await _dbService.Table<Symptom>().ToListAsync();
        }

        public async Task<Symptom?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<Symptom>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(Symptom medicine)
        {
            return await _dbService.UpdateAsync(medicine) > 0;
        }

        public async Task<int> AddAsync(Symptom medicine)
        {
            await _dbService.InsertAsync(medicine);
            return medicine.Id;
        }

        public async Task<bool> DeleteAsync(Symptom medicine)
        {
            return await _dbService.DeleteAsync<Symptom>(medicine.Id) > 0;
        }
    }
}
