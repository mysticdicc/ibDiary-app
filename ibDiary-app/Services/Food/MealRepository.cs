using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Stats;
using ibDiary_data.Data;
using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Food
{
    public class MealRepository : IDatabaseService<Meal>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calService;
        private readonly StatsGenerationService _statsGenerator;

        public MealRepository(
            AppDbContext connection, 
            CalendarDayGenerationService cal,
            StatsGenerationService stats
            )
        {
            _dbService = connection;
            _calService = cal;
            _statsGenerator = stats;
        }

        public async Task<List<Meal>> GetAllAsync()
        {
            return await _dbService.Meals.ToListAsync();
        }

        public async Task<Meal?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<Meal>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(Meal meal)
        {
            var dbItem = await GetByIdAsync(meal.Id);
            if (null == dbItem) return false;

            dbItem.UpdateProperties(meal);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(Meal meal)
        {
            meal.IsNew = false;
            await _dbService.Meals.AddAsync(meal);
            await _dbService.SaveChangesAsync();

            await _calService.NotifyUpdateCalendarDayAsync(meal);
            await _statsGenerator.RequestStatsUpdateAsync();

            return meal.Id;
        }

        public async Task<bool> DeleteAsync(Meal meal)
        {
            var dbItem = await GetByIdAsync(meal.Id);
            if (null == dbItem) return false;

            _dbService.Meals.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }
    }
}
