using ibDiary_data.Models.Food;
using ibDiary_data.Models.Food.Dto;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Food
{
    public class MealClientService(MealRepository repo, ClientNotificationService notificationService, DtoMappingService dtoService) : IDatabaseService<MealDto>
    {
        private readonly MealRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notificationService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<MealDto>> GetAllAsync()
        {
            try
            {
                var list = await _repo.GetAllAsync();
                return _dtoService.ToDtoList(list);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<MealDto?> GetByIdAsync(int id)
        {
            try
            {
                var meal = await _repo.GetByIdAsync(id);
                if (meal == null) return null;

                return _dtoService.ToDto(meal);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(MealDto meal)
        {
            try
            {
                var item = _dtoService.FromDto(meal);
                var result = await _repo.UpdateAsync(item);

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

        public async Task<int> AddAsync(MealDto meal)
        {
            try
            {
                var item = _dtoService.FromDto(meal);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Meal", "No changes were made to the meal.");
                else _notifier.ShowNotification("Add Meal", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MealDto meal)
        {
            try
            {
                var item = _dtoService.FromDto(meal);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Meal", "No changes were made to the database.");
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