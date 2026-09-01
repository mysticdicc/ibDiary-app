using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class MedicineOccuranceClientService(MedicineOccuranceRepository repo, ClientNotificationService notifService) : IDatabaseService<MedicineDueAtOccurance>
    {
        private readonly MedicineOccuranceRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<MedicineDueAtOccurance>> GetAllAsync()
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

        public async Task<MedicineDueAtOccurance?> GetByIdAsync(int id)
        {
            try
            {
                var occurance = await _repo.GetByIdAsync(id);
                return occurance;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(MedicineDueAtOccurance occurance)
        {
            try
            {
                var result = await _repo.UpdateAsync(occurance);

                if (!result) _notifier.ShowNotification("Update Medicine Occurance", "No changes were made to the medicine occurance.");
                else _notifier.ShowNotification("Update Medicine Occurance", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(MedicineDueAtOccurance occurance)
        {
            try
            {
                var result = await _repo.AddAsync(occurance);

                if (result == 0) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Medicine Occurance Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MedicineDueAtOccurance occurance)
        {
            try
            {
                var result = await _repo.DeleteAsync(occurance);

                if (!result) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Medicine Occurance", "Deleted successfully.");

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