using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Medication
{
    public class MedicineReport : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime MedicineTakenAt { get; set; }
        [NotMapped] public DateOnly MedicineTakenAtDate { get  => DateOnly.FromDateTime(MedicineTakenAt); }
        public DateTime? DueAt { get; set; }
        public bool MedicineTaken { get; set; }
        [MaxLength(128, ErrorMessage = "Dose must not exceed 128 characters.")]
        public string Dose { get; set; }
        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }
        public bool IsNew { get; set; }

        public MedicineReport()
        {
            Id = 0;
            Medicine = new();
            SubmittedAt = DateTime.UtcNow;
            MedicineTakenAt = SubmittedAt;
            MedicineTaken = true;
            Dose = string.Empty;
            Notes = string.Empty;
            IsNew = true;
        }

        public MedicineReport(Medicine medicine, DateTime dueDate)
        {
            Id = 0;
            Medicine = medicine;
            MedicineId = medicine.Id;
            DueAt = dueDate;
            MedicineTakenAt = DateTime.UtcNow;
            SubmittedAt = DateTime.UtcNow;
            MedicineTaken = true;
            Dose = medicine.Dose;
            Notes = string.Empty;
            IsNew = true;
        }

        public void UpdateProperties(MedicineReport report)
        {
            Medicine = report.Medicine;
            MedicineId = report.MedicineId;
            MedicineTakenAt = report.MedicineTakenAt;
            MedicineTaken = report.MedicineTaken;
            Notes = report.Notes;
            DueAt = report.DueAt;
        }

        public DateOnly GetDate() => MedicineTakenAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (GetDate() != day.Date) return;
            if (!day.MedicineReports.Contains(this)) day.MedicineReports.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = MedicineTakenAt.Minute.ToString("D2");
            list.Add($"You took {Dose} of {Medicine.Name} at {MedicineTakenAt.Hour}:{minute}.");
            return list;
        }
    }
}
