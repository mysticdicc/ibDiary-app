using ibDiary_app.Data;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomStateChangeRepository : IDatabaseService<SymptomActiveStateChange>
    {
        private readonly AppDbContext _dbService;

        public SymptomStateChangeRepository(AppDbContext connection)
        {
            _dbService = connection;
        }

        public async Task<List<SymptomActiveStateChange>> GetAllAsync()
        {
            return await _dbService.SymptomStateChanges.ToListAsync();
        }

        public async Task<SymptomActiveStateChange?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<SymptomActiveStateChange>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(SymptomActiveStateChange symptom)
        {
            var dbItem = await GetByIdAsync(symptom.Id);
            if (dbItem == null) return false;

            dbItem = symptom;
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(SymptomActiveStateChange symptom)
        {
            await _dbService.SymptomStateChanges.AddAsync(symptom);
            await _dbService.SaveChangesAsync();
            return symptom.Id;
        }

        public async Task<bool> DeleteAsync(SymptomActiveStateChange symptom)
        {
            var dbItem = await GetByIdAsync(symptom.Id);
            if (dbItem == null) return false;

            _dbService.SymptomStateChanges.Remove(symptom);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
