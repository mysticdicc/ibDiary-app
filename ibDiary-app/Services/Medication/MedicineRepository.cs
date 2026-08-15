using ibDiary_app.Data;
using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Medication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services
{
    public class MedicineRepository : IDatabaseService<Medicine>
    {
        private readonly AppDbContext _dbService;
        private readonly MedicineStateChangeRepository _medStateChangeRepo;

        public MedicineRepository(AppDbContext connection, MedicineStateChangeRepository repo)
        {
            _dbService = connection;
            _medStateChangeRepo = repo;
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            return await _dbService.Medicines.Include(x => x.MedicineSchedule).Include(x => x.StateChanges).ToListAsync();
        }

        public async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _dbService.Medicines.Include(x => x.MedicineSchedule).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> UpdateAsync(Medicine medicine)
        {
            var dbItem = await GetByIdAsync(medicine.Id);
            if (dbItem == null) return false;

            var entry = _dbService.Entry(dbItem);
            var clone = Medicine.FromDbEntry(
                dbItem.Id,
                _dbService.Entry(dbItem).OriginalValues
            );
            clone.MedicineSchedule = dbItem.MedicineSchedule;
            _dbService.Entry(clone).State = EntityState.Detached;

            if (dbItem.HasChangedState(clone))
            {
                var stateChange = new MedicineStateChange(medicine.Clone(), clone);
                await _medStateChangeRepo.AddAsync(stateChange);
            }

            dbItem.UpdateProperties(medicine);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(Medicine medicine)
        {
            medicine.IsNew = false;

            medicine.MedicineSchedule.IsNew = false;
            await _dbService.MedicineSchedules.AddAsync(medicine.MedicineSchedule);
            await _dbService.SaveChangesAsync();

            medicine.MedicineScheduleId = medicine.MedicineSchedule.Id;
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
