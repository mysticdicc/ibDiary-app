using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class MedicineClientService(MedicineRepository repo, ClientNotificationService notifService) : IDatabaseService<Medicine>
    {
        private readonly MedicineRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<Medicine>> GetAllAsync()
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

        public async Task<Medicine?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(Medicine medicine)
        {
            try
            {
                var result = await _repo.UpdateAsync(medicine);

                if (!result) _notifier.ShowNotification("Update Medicine", "No changes were made to the medicine.");
                else _notifier.ShowNotification("Update Medicine", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(Medicine medicine)
        {
            try
            {
                var result = await _repo.AddAsync(medicine);

                if (result == 0) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Medicine Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(Medicine medicine)
        {
            try
            {
                var result = await _repo.DeleteAsync(medicine);

                if (!result) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Medicine", "Deleted successfully.");

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
