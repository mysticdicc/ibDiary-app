using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Settings;
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

                var allReports = await _reportService.GetAllAsync();
                var pendingReports = new List<MedicineReport>();

                foreach (var medicine in activeMedicines)
                {
                    var lastReport = allReports
                        .Where(r => r.MedicineId == medicine.Id)
                        .OrderByDescending(r => r.SubmittedAt)
                        .FirstOrDefault();

                    var nextDueDate = await CalculateNextDueDate(lastReport, medicine.MedicineSchedule);
                    if (null != nextDueDate)
                    {
                        if (nextDueDate <= DateTime.UtcNow && (lastReport == null || lastReport.SubmittedAt < nextDueDate))
                        {
                            pendingReports.Add(new(medicine, nextDueDate.Value));
                        }
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

        private async Task<DateTime?> CalculateNextDueDate(MedicineReport? lastReport, MedicineSchedule schedule)
        {
            if (schedule.Type == MedicineScheduleType.AsNeeded) return null;
            else if (schedule.Type == MedicineScheduleType.Interval)
            {
                if (lastReport == null) return DateTime.UtcNow;
                DateTime? nextDue = null;

                if (schedule.IntervalType == ScheduleIntervalType.Minutes)
                {
                    nextDue = lastReport.MedicineTakenAt.AddMinutes(schedule.IntervalValue);
                }
                else if (schedule.IntervalType == ScheduleIntervalType.Hours)
                {
                    nextDue = lastReport.MedicineTakenAt.AddHours(schedule.IntervalValue);
                }
                else if (schedule.IntervalType == ScheduleIntervalType.Days)
                {
                    nextDue = lastReport.MedicineTakenAt.AddDays(schedule.IntervalValue);
                }
                else if (schedule.IntervalType == ScheduleIntervalType.Months)
                {
                    nextDue = lastReport.MedicineTakenAt.AddMonths(schedule.IntervalValue);
                }

                return nextDue;
            }
            else if (schedule.Type == MedicineScheduleType.DailyLimit)
            {
                if (lastReport == null) return DateTime.UtcNow;
                if (null == schedule.Medicine) return null;
                var reports = await _reportService.GetReportsForMedicineOnDateAsync(schedule.Medicine, DateOnly.FromDateTime(DateTime.UtcNow));
               
                if (reports.Count < schedule.AmountPerDay)
                {
                    DateTime? nextDue = null;
                    if (lastReport == null)
                    {
                        nextDue = new DateTime(
                            DateTime.UtcNow.Year,
                            DateTime.UtcNow.Month,
                            DateTime.UtcNow.Day,
                            schedule.StartedAt.Hour,
                            schedule.StartedAt.Minute,
                            schedule.StartedAt.Second,
                            DateTimeKind.Utc
                        );
                    }
                    else if (DateOnly.FromDateTime(lastReport.MedicineTakenAt) < DateOnly.FromDateTime(DateTime.UtcNow))
                    {
                        nextDue = new DateTime(
                            DateTime.UtcNow.Year,
                            DateTime.UtcNow.Month,
                            DateTime.UtcNow.Day,
                            schedule.StartedAt.Hour,
                            schedule.StartedAt.Minute,
                            schedule.StartedAt.Second,
                            DateTimeKind.Utc
                        );
                    }
                    else
                    {
                        nextDue = lastReport.MedicineTakenAt.AddHours(schedule.IntervalValue);
                    }

                    return nextDue;
                }
            }

            return null;
        }
    }
}
