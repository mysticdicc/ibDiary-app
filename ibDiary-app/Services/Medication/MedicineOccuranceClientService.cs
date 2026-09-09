using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication.Dto;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Medication
{
    public class MedicineOccuranceClientService(MedicineOccuranceRepository repo, ClientNotificationService notifService, DtoMappingService dtoService) : IDatabaseService<MedicineDueAtOccuranceDto>
    {
        private readonly MedicineOccuranceRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<MedicineDueAtOccuranceDto>> GetAllAsync()
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

        public async Task<MedicineDueAtOccuranceDto?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repo.GetByIdAsync(id);
                if (item == null) return null;

                return _dtoService.ToDto(item);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(MedicineDueAtOccuranceDto occurance)
        {
            try
            {
                var item = _dtoService.FromDto(occurance);
                var result = await _repo.UpdateAsync(item);

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

        public async Task<int> AddAsync(MedicineDueAtOccuranceDto occurance)
        {
            try
            {
                var item = _dtoService.FromDto(occurance);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Medicine Occurance", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Medicine Occurance", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MedicineDueAtOccuranceDto occurance)
        {
            try
            {
                var item = _dtoService.FromDto(occurance);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Medicine Occurance", "No changes were made to the database.");
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