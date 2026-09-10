using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibDiary_data.Models.Stats
{
    public class MedicineTakenTrendPoint : IUpdatableObject<MedicineTakenTrendPoint>, IMergableListItem<List<MedicineTakenTrendPoint>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public double AverageTaken { get; set; }
        public int ReportCount { get; set; }

        public MedicineTakenTrendPoint()
        {
            Id = 0;
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
            AverageTaken = 0;
            ReportCount = 0;
        }

        public MedicineTakenTrendPoint(DateOnly date)
        {
            Id = 0;
            Date = date;
            AverageTaken = 0;
            ReportCount = 0;
        }

        public void UpdateProperties(MedicineTakenTrendPoint source)
        {
            Date = source.Date;
            AverageTaken = source.AverageTaken;
            ReportCount = source.ReportCount;
        }

        public void MergeToList(List<MedicineTakenTrendPoint> target)
        {
            var existing = target.FirstOrDefault(x => x.Date == Date);
            if (existing == null) target.Add(this);
            else existing.UpdateProperties(this);
        }
    }
}