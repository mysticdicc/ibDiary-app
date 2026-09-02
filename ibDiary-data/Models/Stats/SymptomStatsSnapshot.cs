using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class SymptomStatsSnapshot : IStatsObject<Symptom>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Symptom Symptom { get; set; }
        public int TotalReportsCount { get; set; }
        public int MonthlyReportsCount { get; set; }
        public List<SymptomSeverityTrendPoint> MonthlySeverityTrend { get; set; }
        public int TotalStateChanges { get; set; }
        public int MonthlyStateChanges { get; set; }

        public SymptomStatsSnapshot()
        {
            Id = 0;
            Symptom = new();
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            MonthlySeverityTrend = [];
            TotalStateChanges = 0;
            MonthlyStateChanges = 0;
        }

        public SymptomStatsSnapshot(Symptom symptom)
        {
            Id = 0;
            Symptom = symptom;
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            MonthlySeverityTrend = [];
            TotalStateChanges = 0;
            MonthlyStateChanges = 0;
        }

        public Task GenerateStats(Symptom symptom, DateOnly monthBefore)
        {
            var endDate = monthBefore.AddMonths(1);
            var reports = symptom.SymptomReports;

            TotalReportsCount = reports.Count;
            var monthly = reports.Where(x => x.SubmittedForDate >= monthBefore && x.SubmittedForDate < endDate).ToList();
            MonthlyReportsCount = monthly.Count;

            TotalStateChanges = symptom.StateChanges.Count;
            var monthlySc = symptom.StateChanges.Where(x => x.ChangedAtDate >= monthBefore && x.ChangedAtDate < endDate).ToList();
            MonthlyStateChanges = monthlySc.Count;

            for (var date = monthBefore; date <= endDate; date = date.AddDays(1))
            {
                var point = new SymptomSeverityTrendPoint(date);
                point.GenerateStats(symptom, monthBefore);
                MonthlySeverityTrend.Add(point);
            }

            return Task.CompletedTask;
        }
    }
}
