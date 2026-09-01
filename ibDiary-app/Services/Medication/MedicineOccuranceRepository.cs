using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using Microsoft.EntityFrameworkCore;

namespace ibDiary_app.Services.Medication
{
    public class MedicineOccuranceRepository : IDatabaseService<MedicineDueAtOccurance>
    {
        private readonly AppDbContext _dbService;

        public MedicineOccuranceRepository(AppDbContext connection)
        {
            _dbService = connection;
        }

        public async Task<List<MedicineDueAtOccurance>> GetAllAsync()
        {
            return await _dbService
                .Set<MedicineDueAtOccurance>()
                .Include(x => x.Medicine)
                .ToListAsync();
        }

        public async Task<MedicineDueAtOccurance?> GetByIdAsync(int id)
        {
            return await _dbService
                .Set<MedicineDueAtOccurance>()
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(MedicineDueAtOccurance occurance)
        {
            var dbItem = await GetByIdAsync(occurance.Id);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(occurance);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(MedicineDueAtOccurance occurance)
        {
            await _dbService.Set<MedicineDueAtOccurance>().AddAsync(occurance);
            await _dbService.SaveChangesAsync();
            return occurance.Id;
        }

        public async Task<bool> DeleteAsync(MedicineDueAtOccurance occurance)
        {
            var dbItem = await GetByIdAsync(occurance.Id);
            if (dbItem == null) return false;

            _dbService.Set<MedicineDueAtOccurance>().Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}