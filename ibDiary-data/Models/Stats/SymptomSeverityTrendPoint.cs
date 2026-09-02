using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Stats
{
    public class SymptomSeverityTrendPoint : IStatsObject<Symptom>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public double AverageSeverity { get; set; }
        public int ReportCount { get; set; }

        public SymptomSeverityTrendPoint(DateOnly date)
        {
            Id = 0;
            Date = date;
            AverageSeverity = 0;
            ReportCount = 0;
        }

        public void GenerateStats(Symptom symptom, DateTime monthBefore)
        {
            var reports = symptom.SymptomReports.Where(x => x.SubmittedForDate == Date).ToList();
            ReportCount = reports.Count;
            var severity = reports.Select(x => x.Severity).ToList();
            AverageSeverity = severity.Count == 0 ? 0 : severity.Average();
        }
    }
}
