using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomStateChangeClientService(SymptomStateChangeRepository repo, ClientNotificationService notifier) : IDatabaseService<SymptomStateChange>
    {
        private readonly SymptomStateChangeRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifier;

        public async Task<int> AddAsync(SymptomStateChange item)
        {
            try
            {
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("State Change Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(SymptomStateChange item)
        {
            try
            {
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("State Change Deleted", "Deleted successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<SymptomStateChange>> GetAllAsync()
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

        public async Task<SymptomStateChange?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(SymptomStateChange item)
        {
            throw new NotImplementedException("state changes should not be edited via client.");
        }
    }
}
