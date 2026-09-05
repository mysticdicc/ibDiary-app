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
    public class FoodItemRepository : IDatabaseService<FoodItem>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calService;
        private readonly StatsGenerationService _statsGenerator;

        public FoodItemRepository(
            AppDbContext connection, 
            CalendarDayGenerationService cal,
            StatsGenerationService stats
            )
        {
            _dbService = connection;
            _calService = cal;
            _statsGenerator = stats;
        }

        public async Task<List<FoodItem>> GetAllAsync()
        {
            return await _dbService.FoodItems.ToListAsync();
        }

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<FoodItem>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(FoodItem foodItem)
        {
            var dbItem = await GetByIdAsync(foodItem.Id);
            if (null == dbItem) return false;

            dbItem.UpdateProperties(foodItem);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }

        public async Task<int> AddAsync(FoodItem foodItem)
        {
            foodItem.IsNew = false;
            await _dbService.FoodItems.AddAsync(foodItem);
            await _dbService.SaveChangesAsync();

            await _calService.NotifyUpdateCalendarDayAsync(foodItem);
            await _statsGenerator.RequestStatsUpdateAsync();

            return foodItem.Id;
        }

        public async Task<bool> DeleteAsync(FoodItem foodItem)
        {
            var dbItem = await GetByIdAsync(foodItem.Id);
            if (null == dbItem) return false;

            _dbService.FoodItems.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();

            await _statsGenerator.RequestStatsUpdateAsync();

            return rows > 0;
        }
    }
}
