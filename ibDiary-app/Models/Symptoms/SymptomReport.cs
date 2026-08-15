using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Symptoms
{
    public class SymptomReport : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public Symptom Symptom { get; set; }
        public Medicine? Medication { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime SubmittedFor { get; set; }
        [NotMapped] public DateOnly SubmittedForDate { get => DateOnly.FromDateTime(SubmittedFor); }
        public int Severity { get; set; }
        public bool IsNew { get; set; }

        public SymptomReport()
        {
            Id = 0;
            Symptom = new();
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Severity = 0;
            IsNew = true;
        }

        public SymptomReport(Symptom symptom)
        {
            Id = 0;
            Symptom = symptom;
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Severity = 0;
            IsNew = true;
        }

        public void UpdateProperties(SymptomReport report)
        {
            Medication = report.Medication;
            Severity = report.Severity;
        }

        public DateOnly GetDate() => SubmittedForDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.SymptomReports.Contains(this)) day.SymptomReports.Add(this);
        }
    }
}
