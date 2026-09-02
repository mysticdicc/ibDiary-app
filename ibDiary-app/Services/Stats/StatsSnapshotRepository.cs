using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Stats;
using Microsoft.EntityFrameworkCore;

namespace ibDiary_app.Services.Stats
{
    public class StatsSnapshotRepository : IDatabaseService<StatsSnapshot>
    {
        private readonly AppDbContext _dbService;

        public StatsSnapshotRepository(AppDbContext connection)
        {
            _dbService = connection;
        }

        private IQueryable<StatsSnapshot> GetSnapshotQuery()
        {
            return _dbService.Set<StatsSnapshot>()
                .Include(x => x.MedicineStats)
                    .ThenInclude(x => x.Medicine)
                .Include(x => x.MedicineStats)
                    .ThenInclude(x => x.MedicineTakenTrend)
                .Include(x => x.SymptomStats)
                    .ThenInclude(x => x.Symptom)
                .Include(x => x.SymptomStats)
                    .ThenInclude(x => x.MonthlySeverityTrend)
                .Include(x => x.FoodStats)
                    .ThenInclude(x => x.Food)
                .Include(x => x.FoodStats)
                    .ThenInclude(x => x.FoodEatenByHour)
                .Include(x => x.MealStats)
                    .ThenInclude(x => x.Meal)
                .Include(x => x.MealStats)
                    .ThenInclude(x => x.MealEatenByHour);
        }

        public async Task<List<StatsSnapshot>> GetAllAsync()
        {
            return await GetSnapshotQuery().ToListAsync();
        }

        public async Task<StatsSnapshot?> GetByIdAsync(int id)
        {
            return await GetSnapshotQuery().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(StatsSnapshot snapshot)
        {
            var dbItem = await GetByIdAsync(snapshot.Id);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(snapshot);

            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(StatsSnapshot snapshot)
        {
            await _dbService.Set<StatsSnapshot>().AddAsync(snapshot);
            await _dbService.SaveChangesAsync();

            return snapshot.Id;
        }

        public async Task<bool> DeleteAsync(StatsSnapshot snapshot)
        {
            var dbItem = await GetByIdAsync(snapshot.Id);
            if (dbItem == null) return false;

            _dbService.Set<StatsSnapshot>().Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<StatsSnapshot?> GetByDateAsync(DateOnly date)
        {
            var snapshot = await GetSnapshotQuery().Where(x => x.MonthEnd == date).FirstOrDefaultAsync();
            return snapshot ?? null;
        }
    }
}