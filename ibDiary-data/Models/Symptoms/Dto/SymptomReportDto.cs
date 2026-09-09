using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Medication.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Symptoms.Dto
{
    public class SymptomReportDto : ICalendarUpdate
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

        public SymptomReportDto(SymptomDto symptom)
        {
            Id = 0;
            Symptom = symptom;
            Medication = null;
            SubmittedAtLocal = DateTime.Now;
            SubmittedForLocal = SubmittedAtLocal;
            Severity = 0;
            Notes = string.Empty;
            IsNew = true;
        }

        public DateOnly GetDate() => SubmittedForLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = SubmittedForLocal.Minute.ToString("D2");
            list.Add($"You reported that your {Symptom.Title} symptom was severity {Severity} at {SubmittedForLocal.Hour}:{minute}.");
            return list;
        }

        public SymptomReport Clone()
        {
            var clone = new SymptomReport();

            foreach (var property in typeof(SymptomReport).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            return clone;
        }
    }
}
