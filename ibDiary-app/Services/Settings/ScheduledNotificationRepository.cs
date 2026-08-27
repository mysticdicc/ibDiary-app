using ibDiary_data.Data;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Settings;
using ibDiary_app.Services.Calendar;
using Microsoft.EntityFrameworkCore;

namespace ibDiary_app.Services.Settings
{
    public class ScheduledNotificationRepository : IDatabaseService<ScheduledNotification>
    {
        private readonly AppDbContext _dbService;
        private readonly CalendarDayGenerationService _calendarService;

        public ScheduledNotificationRepository(AppDbContext connection, CalendarDayGenerationService cal)
        {
            _dbService = connection;
            _calendarService = cal;
        }

        public async Task<List<ScheduledNotification>> GetAllAsync()
        {
            return await _dbService.ScheduledNotifications.ToListAsync();
        }

        public async Task<ScheduledNotification?> GetByIdAsync(int id)
        {
            return await _dbService.FindAsync<ScheduledNotification>(id) ?? null;
        }

        public async Task<bool> UpdateAsync(ScheduledNotification notification)
        {
            var dbItem = await GetByIdAsync(notification.Id);
            if (dbItem == null) return false;

            dbItem.UpdateProperties(notification);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<int> AddAsync(ScheduledNotification notification)
        {
            notification.IsNew = false;
            await _dbService.ScheduledNotifications.AddAsync(notification);
            await _dbService.SaveChangesAsync();

            await _calendarService.NotifyUpdateCalendarDayAsync(notification);

            return notification.Id;
        }

        public async Task<bool> DeleteAsync(ScheduledNotification notification)
        {
            var dbItem = await GetByIdAsync(notification.Id);
            if (dbItem == null) return false;

            _dbService.ScheduledNotifications.Remove(dbItem);
            var rows = await _dbService.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<List<ScheduledNotification>> GetActiveNotificationsAsync()
        {
            return await _dbService.ScheduledNotifications
                .Where(x => x.Active)
                .ToListAsync();
        }

        public async Task<List<ScheduledNotification>> GetNotificationsByTypeAsync(ScheduledNotificationType type)
        {
            return await _dbService.ScheduledNotifications
                .Where(x => x.Type == type)
                .ToListAsync();
        }
    }
}