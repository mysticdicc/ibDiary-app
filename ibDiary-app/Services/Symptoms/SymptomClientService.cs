using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomClientService(SymptomRepository repo, ClientNotificationService notificationService) : IDatabaseService<Symptom>
    {
        private readonly SymptomRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notificationService;

        public async Task<List<Symptom>> GetAllAsync()
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

        public async Task<Symptom?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(Symptom symptom)
        {
            try
            {
                var result = await _repo.UpdateAsync(symptom);

                if (!result) _notifier.ShowNotification("Update Symptom", "No changes were made to the report.");
                else _notifier.ShowNotification("Update Symptom", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(Symptom symptom)
        {
            try
            {
                var result = await _repo.AddAsync(symptom);

                if (result == 0) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Symptom Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(Symptom symptom)
        {
            try
            {
                var result = await _repo.DeleteAsync(symptom);

                if (!result) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Symptom", "Deleted successfully.");

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
