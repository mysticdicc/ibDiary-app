using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class MedicineStatsSnapshot : IUpdatableObject<MedicineStatsSnapshot>, IMergableListItem<List<MedicineStatsSnapshot>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Medicine Medicine { get; set; }
        public int TotalReportsCount { get; set; }
        public int MonthlyReportsCount { get; set; }
        public List<MedicineTakenTrendPoint> MedicineTakenTrend { get; set; }
        public int TotalStateChanges { get; set; }
        public int MonthlyStateChanges { get; set; }

        public MedicineStatsSnapshot()
        {
            Id = 0;
            Medicine = new();
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            MedicineTakenTrend = [];
            TotalStateChanges = 0;
            MonthlyStateChanges = 0;
        }

        public MedicineStatsSnapshot(Medicine medicine)
        {
            Id = 0;
            Medicine = medicine;
            TotalReportsCount = 0;
            MonthlyReportsCount = 0;
            MedicineTakenTrend = [];
            TotalStateChanges = 0;
            MonthlyStateChanges = 0;
        }

        public void UpdateProperties(MedicineStatsSnapshot source)
        {
            TotalReportsCount = source.TotalReportsCount;
            MonthlyReportsCount = source.MonthlyReportsCount;
            TotalStateChanges = source.TotalStateChanges;
            MonthlyStateChanges = source.MonthlyStateChanges;

            foreach (var item in source.MedicineTakenTrend)
            {
                item.MergeToList(MedicineTakenTrend);
            }

            MedicineTakenTrend.RemoveAll(existing =>
                !source.MedicineTakenTrend.Any(x => x.Date == existing.Date));
        }

        public void MergeToList(List<MedicineStatsSnapshot> target)
        {
            var existing = target.FirstOrDefault(x => x.Medicine.Id == Medicine.Id);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}