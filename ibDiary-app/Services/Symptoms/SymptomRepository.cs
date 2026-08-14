using ibDiary_app.Data;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ibDiary_app.Services
{
    public class SymptomRepository : IDatabaseService<Symptom>
    {
        private readonly AppDbContext _dbService;
        private readonly SymptomStateChangeRepository _sympStateChangeRepo;

        public SymptomRepository(AppDbContext connection, SymptomStateChangeRepository sympStateChangeRepo)
        {
            _dbService = connection;
            _sympStateChangeRepo = sympStateChangeRepo;
        }

        public async Task<List<Symptom>> GetAllAsync()
        {
            return await _dbService.Symptoms.Include(x => x.StateChanges).ToListAsync();
        }

        public async Task<Symptom?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<Symptom>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(Symptom symptom)
        {
            var dbItem = await GetByIdAsync(symptom.Id);
            if (null == dbItem) throw new Exception("Cannot find in database to update.");

            var entry = _dbService.Entry(dbItem);
            var clone = Symptom.FromDbEntry(
                dbItem.Id,
                _dbService.Entry(dbItem).OriginalValues
            );
            _dbService.Entry(clone).State = EntityState.Detached;

            if (dbItem.HasChangedState(clone))
            {
                var stateChange = new SymptomStateChange(symptom.Clone(), clone);
                await _sympStateChangeRepo.AddAsync(stateChange);
            }

            dbItem.UpdateProperties(symptom);
            var rows = await _dbService.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(Symptom symptom)
        {
            symptom.IsNew = false;
            await _dbService.Symptoms.AddAsync(symptom);
            await _dbService.SaveChangesAsync();
            return symptom.Id;
        }

        public async Task<bool> DeleteAsync(Symptom symptom)
        {
            var dbItem = await GetByIdAsync(symptom.Id);
            if (null == dbItem) return false;

            _dbService.Symptoms.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
