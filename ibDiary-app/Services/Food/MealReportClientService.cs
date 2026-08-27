using ibDiary_data.Models.Food;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Food
{
    public class MealReportClientService(MealReportRepository repo, ClientNotificationService notifService) : IDatabaseService<MealReport>
    {
        private readonly MealReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<MealReport>> GetAllAsync()
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

        public async Task<MealReport?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(MealReport report)
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

        public async Task<int> AddAsync(MealReport report)
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

        public async Task<bool> DeleteAsync(MealReport report)
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
