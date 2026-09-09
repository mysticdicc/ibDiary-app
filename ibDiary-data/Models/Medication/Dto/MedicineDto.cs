using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineDto : ICalendarUpdate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Name must not exceed 128 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Dose cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Dose must not exceed 128 characters.")]
        public string Dose { get; set; }

        [Required(ErrorMessage = "Prescribed by cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Prescribed by must not exceed 128 characters.")]
        public string PrescribedBy { get; set; }

        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }

        public DateTime PrescribedAtLocal { get; set; }
        public DateOnly PrescribedAtLocalDate { get => DateOnly.FromDateTime(PrescribedAtLocal); }

        public int MedicineScheduleId { get; set; }
        public MedicineScheduleDto MedicineSchedule { get; set; }

        public List<MedicineStateChangeDto> StateChanges { get; set; }
        public List<MedicineDueAtOccuranceDto> MedicineOccurances { get; set; }

        public bool Active { get; set; }
        public bool IsNew { get; set; }
        public List<MedicineReportDto> MedicineReports { get; set; }

        public MedicineDto()
        {
            Id = 0;
            Name = string.Empty;
            Dose = string.Empty;
            PrescribedBy = string.Empty;
            Notes = string.Empty;
            PrescribedAtLocal = DateTime.Now;
            Active = true;
            StateChanges = [];
            MedicineOccurances = [];
            IsNew = true;
            MedicineSchedule = new();
            MedicineReports = [];
        }

        public DateOnly GetDate() => PrescribedAtLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            list.Add($"You were prescribed {Dose} of {Name} by {PrescribedBy}.");
            return list;
        }
    }
}
