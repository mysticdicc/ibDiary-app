using ibDiary_app.Data;
using ibDiary_app.Models.Food;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Services.Calendar;
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

        public FoodItemRepository(AppDbContext connection, CalendarDayGenerationService cal)
        {
            _dbService = connection;
            _calService = cal;
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

            return rows > 0;
        }

        public async Task<int> AddAsync(FoodItem foodItem)
        {
            foodItem.IsNew = false;
            await _dbService.FoodItems.AddAsync(foodItem);
            await _dbService.SaveChangesAsync();

            await _calService.NotifyUpdateCalendarDayAsync(foodItem);

            return foodItem.Id;
        }

        public async Task<bool> DeleteAsync(FoodItem foodItem)
        {
            var dbItem = await GetByIdAsync(foodItem.Id);
            if (null == dbItem) return false;

            _dbService.FoodItems.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }
    }
}
