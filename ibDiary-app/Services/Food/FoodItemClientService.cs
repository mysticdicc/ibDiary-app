using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;
using ibDiary_data.Models.Food.Dto;

namespace ibDiary_app.Services.Food
{
    public class FoodItemClientService(FoodItemRepository repo, ClientNotificationService notifService, DtoMappingService dtoService) : IDatabaseService<FoodItemDto>
    {
        private readonly FoodItemRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<FoodItemDto>> GetAllAsync()
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

        public async Task<FoodItemDto?> GetByIdAsync(int id)
        {
            try
            {
                var foodItem = await _repo.GetByIdAsync(id);
                if (foodItem == null) return null;
                return _dtoService.ToDto(foodItem);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(FoodItemDto foodItem)
        {
            try
            {
                var item = _dtoService.FromDto(foodItem);
                var result = await _repo.UpdateAsync(item);

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

        public async Task<int> AddAsync(FoodItemDto foodItem)
        {
            try
            {
                var item = _dtoService.FromDto(foodItem);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Food Item", "Unspecified error, no changes were made to the database.");
                else _notifier.ShowNotification("Add Food Item", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(FoodItemDto foodItem)
        {
            try
            {
                var item = _dtoService.FromDto(foodItem);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Food Item", "Unspecified error, no changes were made to the database.");
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
