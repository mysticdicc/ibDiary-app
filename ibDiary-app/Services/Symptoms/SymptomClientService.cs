using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using ibDiary_data.Models.Symptoms.Dto;
using ibDiary_app.Services.System;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomClientService(
        SymptomRepository repo,
        ClientNotificationService notificationService,
        DtoMappingService dtoService) : IDatabaseService<SymptomDto>
    {
        private readonly SymptomRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notificationService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<SymptomDto>> GetAllAsync()
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

        public async Task<SymptomDto?> GetByIdAsync(int id)
        {
            try
            {
                var report = await _repo.GetByIdAsync(id);
                if (report == null) return null;

                return _dtoService.ToDto(report);
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(SymptomDto symptom)
        {
            try
            {
                var item = _dtoService.FromDto(symptom);
                var result = await _repo.UpdateAsync(item);

                if (!result) _notifier.ShowNotification("Update Symptom", "No changes were made to the symptom.");
                else _notifier.ShowNotification("Update Symptom", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(SymptomDto symptom)
        {
            try
            {
                var item = _dtoService.FromDto(symptom);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Symptom", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Symptom", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(SymptomDto symptom)
        {
            try
            {
                var item = _dtoService.FromDto(symptom);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Symptom", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Symptom", "Deleted successfully.");

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