using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Food
{
    public class MealClientService(MealRepository repo, ClientNotificationService notificationService) : IDatabaseService<Meal>
    {
        private readonly MealRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notificationService;

        public async Task<List<Meal>> GetAllAsync()
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

        public async Task<Meal?> GetByIdAsync(int id)
        {
            try
            {
                var meal = await _repo.GetByIdAsync(id);
                return meal;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Meal meal)
        {
            try
            {
                var result = await _repo.UpdateAsync(meal);

                if (!result) _notifier.ShowNotification("Update Meal", "No changes were made to the meal.");
                else _notifier.ShowNotification("Update Meal", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(Meal meal)
        {
            try
            {
                var result = await _repo.AddAsync(meal);

                if (result == 0) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Meal Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(Meal meal)
        {
            try
            {
                var result = await _repo.DeleteAsync(meal);

                if (!result) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Meal", "Deleted successfully.");

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
