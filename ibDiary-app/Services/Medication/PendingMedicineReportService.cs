using ibDiary_app.Models.Medication;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Medication
{
    public class PendingMedicineReportService
    {
        private readonly MedicineReportDatabaseService _reportService;
        private readonly MedicineDatabaseService _medicineService;
        private readonly NotificationService _notifier;

        public PendingMedicineReportService(
            MedicineReportDatabaseService reportService,
            MedicineDatabaseService medicineService,
            NotificationService notifier)
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

                var allReports = await _reportService.GetAllAsync();
                var pendingReports = new List<MedicineReport>();

                foreach (var medicine in activeMedicines)
                {
                    var lastReport = allReports
                        .Where(r => r.MedicineId == medicine.Id)
                        .OrderByDescending(r => r.SubmittedAt)
                        .FirstOrDefault();

                    var nextDueDate = CalculateNextDueDate(lastReport, medicine.MedicineSchedule);

                    if (nextDueDate <= DateTime.UtcNow && (lastReport == null || lastReport.SubmittedAt < nextDueDate))
                    {
                        pendingReports.Add(new(medicine, nextDueDate));
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

        private DateTime CalculateNextDueDate(MedicineReport? lastReport, MedicineSchedule schedule)
        {
            if (lastReport == null)
            {
                return DateTime.UtcNow;
            }

            var nextDue = lastReport.MedicineTakenAt
                .AddDays(schedule.Days)
                .AddHours(schedule.Hours)
                .AddMinutes(schedule.Minutes);

            return nextDue;
        }
    }
}
