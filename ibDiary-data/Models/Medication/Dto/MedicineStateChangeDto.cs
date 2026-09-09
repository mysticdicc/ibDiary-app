using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineStateChangeDto : ICalendarUpdate
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public DateTime ChangedAtLocal { get; set; }
        public DateOnly ChangedAtLocalDate { get => DateOnly.FromDateTime(ChangedAtLocal); }
        public MedicineDto MedicineBefore { get; set; }
        public MedicineDto MedicineAfter { get; set; }
        public bool IsNew { get; set; }

        public MedicineStateChangeDto()
        {
            Id = 0;
            MedicineId = 0;
            ChangedAtLocal = DateTime.Now;
            MedicineBefore = new();
            MedicineAfter = new();
            IsNew = true;
        }

        public DateOnly GetDate() => ChangedAtLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            if (MedicineBefore.Active != MedicineAfter.Active)
            {
                list.Add($"{MedicineAfter.Name} active was changed to {MedicineAfter.Active}.");
            }
            if (MedicineBefore.Name != MedicineAfter.Name)
            {
                list.Add($"{MedicineBefore.Name} was changed to {MedicineAfter.Name}.");
            }
            if (MedicineBefore.Dose != MedicineAfter.Dose)
            {
                list.Add($"{MedicineAfter.Name} dose was changed to {MedicineAfter.Dose}.");
            }
            if (MedicineBefore.MedicineSchedule != MedicineAfter.MedicineSchedule)
            {
                list.Add($"{MedicineAfter.Name} schedule was updated.");
            }
            if (MedicineBefore.PrescribedBy != MedicineAfter.PrescribedBy)
            {
                list.Add($"{MedicineAfter.Name} prescribed by was changed to {MedicineAfter.PrescribedBy}.");
            }
            if (MedicineBefore.PrescribedAtLocalDate != MedicineAfter.PrescribedAtLocalDate)
            {
                list.Add($"{MedicineAfter.Name} prescribed at was updated to {MedicineAfter.PrescribedAtLocalDate}.");
            }
            return list;
        }
    }
}
