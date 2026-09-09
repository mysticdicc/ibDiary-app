using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Settings;
using ibDiary_data.Models.Settings.Dto;
using ibDiary_app.Services.System;

namespace ibDiary_app.Services.Settings
{
    public class ScheduledNotificationClientService(
        ScheduledNotificationRepository repo,
        ClientNotificationService notifService,
        DtoMappingService dtoService) : IDatabaseService<ScheduledNotificationDto>
    {
        private readonly ScheduledNotificationRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<ScheduledNotificationDto>> GetAllAsync()
        {
            try
            {
                var list = await _repo.GetAllAsync();
                return _dtoService.ToDtoList(list);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<ScheduledNotificationDto?> GetByIdAsync(int id)
        {
            try
            {
                var notification = await _repo.GetByIdAsync(id);
                if (notification == null) return null;

                return _dtoService.ToDto(notification);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(ScheduledNotificationDto notification)
        {
            try
            {
                var item = _dtoService.FromDto(notification);
                var result = await _repo.UpdateAsync(item);

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

        public async Task<int> AddAsync(ScheduledNotificationDto notification)
        {
            try
            {
                var item = _dtoService.FromDto(notification);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Notification", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Notification", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(ScheduledNotificationDto notification)
        {
            try
            {
                var item = _dtoService.FromDto(notification);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Notification", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Notification", "Deleted successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<ScheduledNotificationDto>> GetActiveNotificationsAsync()
        {
            try
            {
                var list = await _repo.GetActiveNotificationsAsync();
                return _dtoService.ToDtoList(list);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<List<ScheduledNotificationDto>> GetNotificationsByTypeAsync(ScheduledNotificationType type)
        {
            try
            {
                var list = await _repo.GetNotificationsByTypeAsync(type);
                return _dtoService.ToDtoList(list);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }
    }
}