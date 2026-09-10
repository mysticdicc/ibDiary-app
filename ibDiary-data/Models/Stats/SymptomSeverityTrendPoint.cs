using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class SymptomSeverityTrendPoint : IUpdatableObject<SymptomSeverityTrendPoint>, IMergableListItem<List<SymptomSeverityTrendPoint>>
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