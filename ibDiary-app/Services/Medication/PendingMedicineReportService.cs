using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Settings;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;
using ibDiary_data.Models.Medication.Dto;

namespace ibDiary_app.Services.Medication
{
    public class PendingMedicineReportService
    {
        private readonly MedicineReportRepository _reportService;
        private readonly MedicineRepository _medicineService;
        private readonly ClientNotificationService _notifier;
        private readonly DtoMappingService _dtoService;

        public PendingMedicineReportService(
            MedicineReportRepository reportService,
            MedicineRepository medicineService,
            ClientNotificationService notifier,
            DtoMappingService dtoService)
        {
            _reportService = reportService;
            _medicineService = medicineService;
            _notifier = notifier;
            _dtoService = dtoService;
        }

        public async Task<List<MedicineReportDto>> GetPendingReportsAsync()
        {
            try
            {
                var activeMedicines = await _medicineService.GetAllAsync();
                activeMedicines = activeMedicines.Where(m => m.Active).ToList();

                var pendingReports = new List<MedicineReportDto>();

                foreach (var medicine in activeMedicines)
                {
                    var clone = medicine.Clone();
                    clone.RegenerateOccurances(DateTime.UtcNow);

                    if (clone.MedicineSchedule.Type != MedicineScheduleType.AsNeeded &&
                        clone.HasChangedState(medicine))
                    {
                        await _medicineService.UpdateAsync(clone);
                    }

                    var fresh = await _medicineService.GetByIdAsync(medicine.Id);
                    if (null == fresh) continue;

                    foreach (var occ in fresh.MedicineOccurances.Where(x => x.Status == MedicineDueAtStatus.Pending))
                    {
                        pendingReports.Add(new MedicineReportDto(_dtoService.ToDto(fresh), _dtoService.ToDto(occ)));
                    }
                }

                return pendingReports;
            }
            catch (Exception ex)
            {
                _notifier.ShowNotification("Error", ex.Message);
                return [];
            }
        }
    }
}
