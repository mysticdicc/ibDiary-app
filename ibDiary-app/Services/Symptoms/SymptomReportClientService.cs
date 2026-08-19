using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomReportClientService(SymptomReportRepository repo, ClientNotificationService notifService) : IDatabaseService<SymptomReport>
    {
        private readonly SymptomReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<SymptomReport>> GetAllAsync()
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

        public async Task<SymptomReport?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(SymptomReport report)
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

        public async Task<int> AddAsync(SymptomReport report)
        {
            try
            {
                var result = await _repo.AddAsync(report);

                if (result == 0) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Report Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(SymptomReport report)
        {
            try
            {
                var result = await _repo.DeleteAsync(report);

                if (!result) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
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
