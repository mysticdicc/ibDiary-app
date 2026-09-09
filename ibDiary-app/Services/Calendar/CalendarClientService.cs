using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Calendar.Dto;
using ibDiary_data.Models.Interfaces;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Calendar
{
    public class CalendarClientService(
        CalendarRepositoryService repo,
        ClientNotificationService notifier,
        DtoMappingService dtoService)
    {
        private readonly CalendarRepositoryService _repo = repo;
        private readonly ClientNotificationService _notifier = notifier;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<bool> AddAsync(CalendarDayDto item)
        {
            try
            {
                var result = await _repo.AddAsync(_dtoService.FromDto(item));
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(CalendarDayDto item)
        {
            try
            {
                var result = await _repo.DeleteAsync(_dtoService.FromDto(item));
                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<CalendarDayDto>> GetAllAsync()
        {
            try
            {
                var result = await _repo.GetAllAsync();
                return _dtoService.ToDtoList(result);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<CalendarDayDto?> GetByIdAsync(DateOnly date)
        {
            try
            {
                var result = await _repo.GetByIdAsync(date);
                if (result == null) return null;

                return _dtoService.ToDto(result);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<List<CalendarDayDto>> GetFromDateAsync(DateOnly from)
        {
            try
            {
                var result = await _repo.GetFromDateAsync(from);
                return _dtoService.ToDtoList(result);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }

        public async Task<bool> UpdateAsync(CalendarDayDto item)
        {
            try
            {
                var result = await _repo.UpdateAsync(_dtoService.FromDto(item));
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