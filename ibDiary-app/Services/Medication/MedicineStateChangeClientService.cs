using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication.Dto;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Medication
{
    public class MedicineStateChangeClientService(MedicineStateChangeRepository repo, ClientNotificationService notifier, DtoMappingService dtoService) : IDatabaseService<MedicineStateChangeDto>
    {
        private readonly MedicineStateChangeRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifier;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<int> AddAsync(MedicineStateChangeDto item)
        {
            try
            {
                var result = await _repo.AddAsync(_dtoService.FromDto(item));

                if (result == 0) _notifier.ShowNotification("Add State Change", "No changes were made to the database.");
                else _notifier.ShowNotification("Add State Change", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MedicineStateChangeDto item)
        {
            try
            {
                var result = await _repo.DeleteAsync(_dtoService.FromDto(item));

                if (!result) _notifier.ShowNotification("Delete State Change", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete State Change", "Deleted successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<MedicineStateChangeDto>> GetAllAsync()
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

        public async Task<MedicineStateChangeDto?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(MedicineStateChangeDto item)
        {
            throw new NotImplementedException("state changes should not be edited via client.");
        }
    }
}