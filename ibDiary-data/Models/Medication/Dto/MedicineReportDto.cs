using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineReportDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }

        [NotNewCalendarObject(ErrorMessage = "Medicine is required.")]
        public MedicineDto Medicine { get; set; }

        public DateTime SubmittedAtLocal { get; set; }
        public DateTime MedicineTakenAtLocal { get; set; }
        public DateOnly MedicineTakenAtLocalDate { get => DateOnly.FromDateTime(MedicineTakenAtLocal); }

        public MedicineDueAtOccuranceDto DueAt { get; set; }

        public bool MedicineTaken { get; set; }

        [MaxLength(128, ErrorMessage = "Dose must not exceed 128 characters.")]
        public string Dose { get; set; }

        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }

        public bool IsNew { get; set; }

        public MedicineReportDto()
        {
            Id = 0;
            Medicine = new();
            SubmittedAtLocal = DateTime.Now;
            MedicineTakenAtLocal = SubmittedAtLocal;
            DueAt = new();
            MedicineTaken = true;
            Dose = string.Empty;
            Notes = string.Empty;
            IsNew = true;
        }
    }
}
