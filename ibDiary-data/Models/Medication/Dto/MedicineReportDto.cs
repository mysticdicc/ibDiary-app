using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineReportDto : ICalendarUpdate
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

        public MedicineReportDto(DateTime submittedAt)
        {
            Id = 0;
            Medicine = new();
            SubmittedAtLocal = submittedAt;
            MedicineTakenAtLocal = submittedAt;
            DueAt = new();
            MedicineTaken = true;
            Dose = string.Empty;
            Notes = string.Empty;
            IsNew = true;
        }

        public MedicineReportDto(MedicineDto medicine, MedicineDueAtOccuranceDto dueAt)
        {
            Id = 0;
            Medicine = medicine;
            SubmittedAtLocal = DateTime.Now;
            MedicineTakenAtLocal = SubmittedAtLocal;
            DueAt = dueAt;
            MedicineTaken = true;
            Dose = medicine.Dose;
            Notes = string.Empty;
            IsNew = true;
        }

        public MedicineReportDto(MedicineDto medicine, MedicineDueAtOccuranceDto dueAt, DateTime submittedAt)
        {
            Id = 0;
            Medicine = medicine;
            SubmittedAtLocal = submittedAt;
            MedicineTakenAtLocal = submittedAt;
            DueAt = dueAt;
            MedicineTaken = true;
            Dose = medicine.Dose;
            Notes = string.Empty;
            IsNew = true;
        }

        public DateOnly GetDate() => MedicineTakenAtLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            var minute = MedicineTakenAtLocal.Minute.ToString("D2");
            list.Add($"You took {Dose} of {Medicine.Name} at {MedicineTakenAtLocal.Hour}:{minute}.");
            return list;
        }

    }
}
