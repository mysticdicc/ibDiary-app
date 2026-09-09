using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Medication.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Symptoms.Dto
{
    public class SymptomReportDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Symptom is required.")]
        public SymptomDto Symptom { get; set; }

        public MedicineDto? Medication { get; set; }

        public DateTime SubmittedAtLocal { get; set; }
        public DateTime SubmittedForLocal { get; set; }

        public DateOnly SubmittedForLocalDate { get => DateOnly.FromDateTime(SubmittedForLocal); }

        [Range(0, 10, ErrorMessage = "Severity must be between 0 and 10.")]
        public int Severity { get; set; }

        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }

        public bool IsNew { get; set; }

        public SymptomReportDto()
        {
            Id = 0;
            Symptom = new();
            Medication = null;
            SubmittedAtLocal = DateTime.Now;
            SubmittedForLocal = SubmittedAtLocal;
            Severity = 0;
            Notes = string.Empty;
            IsNew = true;
        }
    }
}
