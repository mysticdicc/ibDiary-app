using ibDiary_app.Models.Food;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Food
{
    public class FoodItemReportClientService(FoodItemReportRepository repo, ClientNotificationService notifService) : IDatabaseService<FoodItemReport>
    {
        private readonly FoodItemReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<FoodItemReport>> GetAllAsync()
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

        public async Task<FoodItemReport?> GetByIdAsync(int id)
        {
            try
            {
                var report = await _repo.GetByIdAsync(id);
                return report;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(FoodItemReport report)
        {
            try
            {
                var result = await _repo.UpdateAsync(report);

                if (!result) _notifier.ShowNotification("Update Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Update Report", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(FoodItemReport report)
        {
            try
            {
                var result = await _repo.AddAsync(report);

                if (result == 0) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Report Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(FoodItemReport report)
        {
            try
            {
                var result = await _repo.DeleteAsync(report);

                if (!result) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Report", "Deleted successfully.");

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
