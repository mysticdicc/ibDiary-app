using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class MedicineReportClientService(MedicineReportRepository repo, ClientNotificationService notifService) : IDatabaseService<MedicineReport>
    {
        private readonly MedicineReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<MedicineReport>> GetAllAsync()
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

        public async Task<MedicineReport?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(MedicineReport report)
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

        public async Task<int> AddAsync(MedicineReport report)
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

        public async Task<bool> DeleteAsync(MedicineReport report)
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
