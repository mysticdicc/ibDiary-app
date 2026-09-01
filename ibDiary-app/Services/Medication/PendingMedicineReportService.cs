using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Settings;
using ibDiary_app.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class PendingMedicineReportService
    {
        private readonly MedicineReportRepository _reportService;
        private readonly MedicineRepository _medicineService;
        private readonly ClientNotificationService _notifier;

        public PendingMedicineReportService(
            MedicineReportRepository reportService,
            MedicineRepository medicineService,
            ClientNotificationService notifier)
        {
            _reportService = reportService;
            _medicineService = medicineService;
            _notifier = notifier;
        }

        public async Task<List<MedicineReport>> GetPendingReportsAsync()
        {
            try
            {
                var activeMedicines = await _medicineService.GetAllAsync();
                activeMedicines = activeMedicines.Where(m => m.Active).ToList();

                var pendingReports = new List<MedicineReport>();

                foreach (var medicine in activeMedicines)
                {
                    var clone = medicine.Clone();
                    clone.RegenerateOccurances(DateTime.UtcNow);

                    if (clone.HasChangedState(medicine))
                    {
                        await _medicineService.UpdateAsync(clone);
                    }

                    var uncompleted = clone.MedicineOccurances.Where(x => x.Status == MedicineDueAtStatus.Pending);

                    foreach (var occ in uncompleted)
                    {
                        pendingReports.Add(new(medicine, occ));
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
