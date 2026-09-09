using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication.Dto;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;

namespace ibDiary_app.Services.Medication
{
    public class MedicineReportClientService(MedicineReportRepository repo, ClientNotificationService notifService, DtoMappingService dtoService) : IDatabaseService<MedicineReportDto>
    {
        private readonly MedicineReportRepository _repo = repo;
        private readonly ClientNotificationService _notifier = notifService;
        private readonly DtoMappingService _dtoService = dtoService;

        public async Task<List<MedicineReportDto>> GetAllAsync()
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

        public async Task<MedicineReportDto?> GetByIdAsync(int id)
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

        public async Task<bool> UpdateAsync(MedicineReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.UpdateAsync(item);

                if (!result) _notifier.ShowNotification("Update Medicine Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Update Medicine Report", "Updated successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return false;
            }
        }

        public async Task<int> AddAsync(MedicineReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.AddAsync(item);

                if (result == 0) _notifier.ShowNotification("Add Medicine Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Add Medicine Report", "Added successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(MedicineReportDto report)
        {
            try
            {
                var item = _dtoService.FromDto(report);
                var result = await _repo.DeleteAsync(item);

                if (!result) _notifier.ShowNotification("Delete Medicine Report", "No changes were made to the report.");
                else _notifier.ShowNotification("Delete Medicine Report", "Deleted successfully.");

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