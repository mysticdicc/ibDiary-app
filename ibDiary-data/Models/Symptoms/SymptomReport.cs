using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Symptoms
{
    public class SymptomReport : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [NotNewCalendarObject(ErrorMessage = "Symptom is required.")]
        public Symptom Symptom { get; set; }
        public Medicine? Medication { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime SubmittedFor { get; set; }
        [NotMapped] public DateOnly SubmittedForDate { get => DateOnly.FromDateTime(SubmittedFor); }
        [Range(0, 10, ErrorMessage = "Severity must be between 0 and 10.")]
        public int Severity { get; set; }
        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }
        public bool IsNew { get; set; }

        public SymptomReport()
        {
            Id = 0;
            Symptom = new();
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Severity = 0;
            Notes = string.Empty;
            IsNew = true;
        }

        public SymptomReport(Symptom symptom)
        {
            Id = 0;
            Symptom = symptom;
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Severity = 0;
            Notes = string.Empty;
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
            if (GetDate() != day.Date) return;
            if (!day.SymptomReports.Contains(this)) day.SymptomReports.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = SubmittedFor.Minute.ToString("D2");
            list.Add($"You reported that your {Symptom.Title} symptom was severity {Severity} at {SubmittedFor.Hour}:{minute}.");
            return list;
        }
    }
}
