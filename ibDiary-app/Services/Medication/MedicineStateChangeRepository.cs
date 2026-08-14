using ibDiary_app.Data;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class MedicineStateChangeRepository : IDatabaseService<MedicineStateChange>
    {
        private readonly AppDbContext _dbService;

        public MedicineStateChangeRepository(AppDbContext connection)
        {
            _dbService = connection;
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
