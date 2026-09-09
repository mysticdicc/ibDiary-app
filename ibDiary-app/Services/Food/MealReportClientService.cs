using ibDiary_data.Models.Food;
using ibDiary_data.Models.Food.Dto;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Food
{
    public class MealReportClientService(MealReportRepository repo, ClientNotificationService notifService, DtoMappingService dtoService) : IDatabaseService<MealReportDto>
    {
        private readonly MealReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<MealReportDto>> GetAllAsync()
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

        public async Task<MealReportDto?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(MealReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.UpdateAsync(item);

                if (!result) _notifier.ShowNotification("Update Meal Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Update Meal Report", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(MealReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Meal Report", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Meal Report", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MealReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Meal Report", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Meal Report", "Deleted successfully.");

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