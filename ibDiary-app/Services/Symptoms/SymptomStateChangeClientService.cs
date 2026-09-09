using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using ibDiary_data.Models.Symptoms.Dto;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.System;
using System;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomStateChangeClientService(
        SymptomStateChangeRepository repo,
        ClientNotificationService notifier,
        DtoMappingService dtoService) : IDatabaseService<SymptomStateChangeDto>
    {
        private readonly SymptomStateChangeRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifier;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<int> AddAsync(SymptomStateChangeDto item)
        {
            try
            {
                var result = await _repo.AddAsync(_dtoService.FromDto(item));

                if (result == 0) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Add State Change", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(SymptomStateChangeDto item)
        {
            try
            {
                var result = await _repo.DeleteAsync(_dtoService.FromDto(item));

                if (!result) _notifier.ShowNotification("Unspecified Error", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete State Change", "Deleted successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<List<SymptomStateChangeDto>> GetAllAsync()
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

        public async Task<SymptomStateChangeDto?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(SymptomStateChangeDto item)
        {
            throw new NotImplementedException("state changes should not be edited via client.");
        }
    }
}