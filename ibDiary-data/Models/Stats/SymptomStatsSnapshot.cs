using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class SymptomStatsSnapshot : IStatsObject<Symptom>, IUpdatableObject<SymptomStatsSnapshot>, IMergableListItem<List<SymptomStatsSnapshot>>
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
            var monthly = reports.Where(x => x.SubmittedForDate > monthBefore && x.SubmittedForDate <= endDate).ToList();
            MonthlyReportsCount = monthly.Count;

            TotalStateChanges = symptom.StateChanges.Count;
            var monthlySc = symptom.StateChanges.Where(x => x.ChangedAtDate > monthBefore && x.ChangedAtDate <= endDate).ToList();
            MonthlyStateChanges = monthlySc.Count;

            MonthlySeverityTrend = [];
            for (var date = monthBefore; date <= endDate; date = date.AddDays(1))
            {
                var point = new SymptomSeverityTrendPoint(date);
                point.GenerateStats(symptom, monthBefore);
                MonthlySeverityTrend.Add(point);
            }

            return Task.CompletedTask;
        }

        public void UpdateProperties(SymptomStatsSnapshot source)
        {
            TotalReportsCount = source.TotalReportsCount;
            MonthlyReportsCount = source.MonthlyReportsCount;
            TotalStateChanges = source.TotalStateChanges;
            MonthlyStateChanges = source.MonthlyStateChanges;

            foreach (var item in source.MonthlySeverityTrend)
            {
                item.MergeToList(MonthlySeverityTrend);
            }

            MonthlySeverityTrend.RemoveAll(existing =>
                !source.MonthlySeverityTrend.Any(x => x.Date == existing.Date));
        }

        public void MergeToList(List<SymptomStatsSnapshot> target)
        {
            var existing = target.FirstOrDefault(x => x.Symptom == Symptom);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}