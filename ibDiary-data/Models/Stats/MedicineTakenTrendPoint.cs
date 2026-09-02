using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ibDiary_data.Models.Stats
{
    public class MedicineTakenTrendPoint : IStatsObject<Medicine>
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

        public Task GenerateStats(Medicine medicine, DateOnly monthBefore)
        {
            var reports = medicine.MedicineReports.Where(x => x.MedicineTakenAtDate == Date).ToList();
            ReportCount = reports.Count;
            var taken = reports.Where(x => x.MedicineTaken).Count();
            AverageTaken = ReportCount == 0 ? 0 : ((double)taken / ReportCount) * 100;

            return Task.CompletedTask;
        }
    }
}
