using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication.Dto;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Medication
{
    public class MedicineClientService(MedicineRepository repo, ClientNotificationService notifService, DtoMappingService dtoService) : IDatabaseService<MedicineDto>
    {
        private readonly MedicineRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<MedicineDto>> GetAllAsync()
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

        public async Task<MedicineDto?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(MedicineDto medicine)
        {
            try
            {
                var item = _dtoService.FromDto(medicine);
                var result = await _repo.UpdateAsync(item);

                if (!result) _notifier.ShowNotification("Update Medicine", "No changes were made to the medicine.");
                else _notifier.ShowNotification("Update Medicine", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(MedicineDto medicine)
        {
            try
            {
                var item = _dtoService.FromDto(medicine);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Medicine", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Medicine", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MedicineDto medicine)
        {
            try
            {
                var item = _dtoService.FromDto(medicine);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Medicine", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Medicine", "Deleted successfully.");

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