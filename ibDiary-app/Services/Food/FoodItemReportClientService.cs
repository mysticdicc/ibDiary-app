using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;
using ibDiary_data.Models.Food.Dto;

namespace ibDiary_app.Services.Food
{
    public class FoodItemReportClientService(FoodItemReportRepository repo, ClientNotificationService notifService, DtoMappingService dtoService) : IDatabaseService<FoodItemReportDto>
    {
        private readonly FoodItemReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<FoodItemReportDto>> GetAllAsync()
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

        public async Task<FoodItemReportDto?> GetByIdAsync(int id)
        {
            try
            {
                var report = await _repo.GetByIdAsync(id);
                if (report == null) return null;
                return _dtoService.ToDto(report);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(FoodItemReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.UpdateAsync(item);

                if (!result) _notifier.ShowNotification("Update Food Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Update Food Report", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(FoodItemReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Food Report", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Food Report", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(FoodItemReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Food Report", "Unspecified error, no changes were made to the database.");
                else _notifier.ShowNotification("Delete Food Report", "Deleted successfully.");

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
