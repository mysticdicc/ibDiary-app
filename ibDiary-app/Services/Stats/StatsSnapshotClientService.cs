using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Stats;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Stats
{
    public class StatsSnapshotClientService(StatsSnapshotRepository repo, ClientNotificationService notifService) : IDatabaseService<StatsSnapshot>
    {
        private readonly StatsSnapshotRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<StatsSnapshot>> GetAllAsync()
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

        public async Task<StatsSnapshot?> GetByIdAsync(int id)
        {
            try
            {
                var snapshot = await _repo.GetByIdAsync(id);
                return snapshot;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(StatsSnapshot snapshot)
        {
            try
            {
                var result = await _repo.UpdateAsync(snapshot);

                if (!result) _notifier.ShowNotification("Update Stats Snapshot", "No changes were made to the stats snapshot.");
                else _notifier.ShowNotification("Update Stats Snapshot", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(StatsSnapshot snapshot)
        {
            try
            {
                var result = await _repo.AddAsync(snapshot);

                if (result == 0) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Stats Snapshot Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(StatsSnapshot snapshot)
        {
            try
            {
                var result = await _repo.DeleteAsync(snapshot);

                if (!result) _notifier.ShowNotification("Unpsecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Stats Snapshot", "Deleted successfully.");

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