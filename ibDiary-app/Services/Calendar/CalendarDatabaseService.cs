using ibDiary_app.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Calendar
{
    public class CalendarDatabaseService(CalendarRepositoryService repo, NotificationService notifier)
    {
        private readonly CalendarRepositoryService _repo = repo;
        private readonly NotificationService _notifier = notifier;

        public async Task<bool> AddAsync(CalendarDay item)
        {
            try
            {
                var result = await _repo.AddAsync(item);
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(CalendarDay item)
        {
            try
            {
                var result = await _repo.DeleteAsync(item);
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<CalendarDay>> GetAllAsync()
        {
            try
            {
                var result = await _repo.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<CalendarDay?> GetByIdAsync(DateOnly date)
        {
            try
            {
                var result = await _repo.GetByIdAsync(date);
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<List<CalendarDay>> GetFromDateAsync(DateOnly from)
        {
            try
            {
                var result = await _repo.GetFromDateAsync(from);
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<bool> UpdateAsync(CalendarDay item)
        {
            try
            {
                var result = await _repo.UpdateAsync(item);
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
