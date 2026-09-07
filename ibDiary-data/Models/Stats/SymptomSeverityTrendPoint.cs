using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class SymptomSeverityTrendPoint : IStatsObject<Symptom>, IUpdatableObject<SymptomSeverityTrendPoint>, IMergableListItem<List<SymptomSeverityTrendPoint>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public double AverageSeverity { get; set; }
        public int ReportCount { get; set; }

        public SymptomSeverityTrendPoint()
        {
            Id = 0;
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
            AverageSeverity = 0;
            ReportCount = 0;
        }

        public SymptomSeverityTrendPoint(DateOnly date)
        {
            Id = 0;
            Date = date;
            AverageSeverity = 0;
            ReportCount = 0;
        }

        public Task GenerateStats(Symptom symptom, DateOnly monthBefore)
        {
            var reports = symptom.SymptomReports.Where(x => x.GetDate() == Date).ToList();
            ReportCount = reports.Count;
            var severity = reports.Select(x => x.Severity).ToList();
            AverageSeverity = severity.Count == 0 ? 0 : severity.Average();

            return Task.CompletedTask;
        }

        public void UpdateProperties(SymptomSeverityTrendPoint source)
        {
            AverageSeverity = source.AverageSeverity;
            ReportCount = source.ReportCount;
        }

        public void MergeToList(List<SymptomSeverityTrendPoint> target)
        {
            var existing = target.FirstOrDefault(x => x.Date == Date);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}