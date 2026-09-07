using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class MedicineStatsSnapshot : IStatsObject<Medicine>, IUpdatableObject<MedicineStatsSnapshot>, IMergableListItem<List<MedicineStatsSnapshot>>
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

        public Task GenerateStats(Medicine medicine, DateOnly monthBefore)
        {
            var endDate = monthBefore.AddMonths(1);
            var reports = medicine.MedicineReports;

            TotalReportsCount = reports.Count;
            var monthly = reports.Where(x => x.GetDate() > monthBefore && x.GetDate() <= endDate).ToList();
            MonthlyReportsCount = monthly.Count;

            TotalStateChanges = medicine.StateChanges.Count;
            var monthlySc = medicine.StateChanges.Where(x => x.GetDate() > monthBefore && x.GetDate() <= endDate).ToList();
            MonthlyStateChanges = monthlySc.Count;

            MedicineTakenTrend = [];
            for (var date = monthBefore; date <= endDate; date = date.AddDays(1))
            {
                var point = new MedicineTakenTrendPoint(date);
                point.GenerateStats(medicine, monthBefore);
                MedicineTakenTrend.Add(point);
            }

            return Task.CompletedTask;
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
            var existing = target.FirstOrDefault(x => x.Medicine == Medicine);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}