using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class SymptomStatsSnapshot : IUpdatableObject<SymptomStatsSnapshot>, IMergableListItem<List<SymptomStatsSnapshot>>
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
            var existing = target.FirstOrDefault(x => x.Symptom.Id == Symptom.Id);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}