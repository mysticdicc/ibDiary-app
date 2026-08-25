using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Settings;
using ibDiary_app.Services.System;

namespace ibDiary_app.Services.Settings
{
    public class ScheduledNotificationClientService(ScheduledNotificationRepository repo, ClientNotificationService notifService) : IDatabaseService<ScheduledNotification>
    {
        private readonly ScheduledNotificationRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;

        public async Task<List<ScheduledNotification>> GetAllAsync()
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

        public async Task<ScheduledNotification?> GetByIdAsync(int id)
        {
            try
            {
                var notification = await _repo.GetByIdAsync(id);
                return notification;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(ScheduledNotification notification)
        {
            try
            {
                var result = await _repo.UpdateAsync(notification);

                if (!result) _notifier.ShowNotification("Update Notification", "No changes were made to the notification.");
                else _notifier.ShowNotification("Update Notification", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(ScheduledNotification notification)
        {
            try
            {
                var result = await _repo.AddAsync(notification);

                if (result == 0) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Notification Added", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(ScheduledNotification notification)
        {
            try
            {
                var result = await _repo.DeleteAsync(notification);

                if (!result) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Notification", "Deleted successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<ScheduledNotification>> GetActiveNotificationsAsync()
        {
            try
            {
                return await _repo.GetActiveNotificationsAsync();
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<List<ScheduledNotification>> GetNotificationsByTypeAsync(ScheduledNotificationType type)
        {
            try
            {
                return await _repo.GetNotificationsByTypeAsync(type);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }
    }
}