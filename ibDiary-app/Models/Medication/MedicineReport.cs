using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Medication
{
    public class MedicineReport
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime MedicineTakenAt { get; set; }
        public DateTime? DueAt { get; set; }
        public bool MedicineTaken { get; set; }
        public string Notes { get; set; }
        public bool IsNew { get; set; }

        public MedicineReport()
        {
            Id = 0;
            Medicine = new();
            SubmittedAt = DateTime.UtcNow;
            MedicineTakenAt = SubmittedAt;
            MedicineTaken = true;
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
        }
    }
}
