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
    public class MedicineRepository : IDatabaseService<Medicine>
    {
        private readonly AppDbContext _dbService;

        public MedicineRepository(AppDbContext connection)
        {
            _dbService = connection;
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            return await _dbService.Medicines.ToListAsync();
        }

        public async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<Medicine>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(Medicine medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            dbItem = medicine;
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(Medicine medicine)
        {
            medicine.IsNew = false;
            await _dbService.Medicines.AddAsync(medicine);
            await _dbService.SaveChangesAsync();
            return medicine.Id;
        }

        public async Task<bool> DeleteAsync(Medicine medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            _dbService.Medicines.Remove(medicine);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
