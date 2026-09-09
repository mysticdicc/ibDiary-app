using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using ibDiary_data.Models.Symptoms.Dto;
using ibDiary_app.Services.System;

namespace ibDiary_app.Services.Symptoms
{
    public class SymptomReportClientService(
        SymptomReportRepository repo,
        ClientNotificationService notifService,
        DtoMappingService dtoService) : IDatabaseService<SymptomReportDto>
    {
        private readonly SymptomReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<SymptomReportDto>> GetAllAsync()
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

        public async Task<SymptomReportDto?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(SymptomReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.UpdateAsync(item);

                if (!result) _notifier.ShowNotification("Update Symptom Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Update Symptom Report", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(SymptomReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Symptom Report", "No changes were made to the database.");
                else _notifier.ShowNotification("Add Symptom Report", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(SymptomReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Symptom Report", "No changes were made to the database.");
                else _notifier.ShowNotification("Delete Symptom Report", "Deleted successfully.");

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