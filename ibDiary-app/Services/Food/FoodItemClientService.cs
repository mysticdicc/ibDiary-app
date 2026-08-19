using ibDiary_app.Models.Food;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Food
{
    public class FoodItemClientService(FoodItemRepository repo, ClientNotificationService notifService) : IDatabaseService<FoodItem>
    {
        private readonly FoodItemRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<FoodItem>> GetAllAsync()
        {
            try
            {
                var list = await _repo.GetAllAsync();
                return list;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            try
            {
                var foodItem = await _repo.GetByIdAsync(id);
                return foodItem;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(FoodItem foodItem)
        {
            try
            {
                var result = await _repo.UpdateAsync(foodItem);

                if (!result) _notifier.ShowNotification("Update Food Item", "No changes were made to the food item.");
                else _notifier.ShowNotification("Update Food Item", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(FoodItem foodItem)
        {
            try
            {
                var result = await _repo.AddAsync(foodItem);

                if (result == 0) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Food Item Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(FoodItem foodItem)
        {
            try
            {
                var result = await _repo.DeleteAsync(foodItem);

                if (!result) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Food Item", "Deleted successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }
    }
}
