using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Medication
{
    public class MedicineStateChange : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [ForeignKey(nameof(MedicineId))] public Medicine? Medicine { get; set; }
        public int MedicineId { get; set; }
        public DateTime ChangedAt { get; set; }
        [NotMapped] public DateOnly ChangedAtDate { get => DateOnly.FromDateTime(ChangedAt); }
        [Column(TypeName = "jsonb")] public Medicine MedicineBefore { get; set; }
        [Column(TypeName = "jsonb")] public Medicine MedicineAfter { get; set; }

        public MedicineStateChange()
        {
            Id = 0;
            MedicineId = 0;
            ChangedAt = DateTime.UtcNow;
            MedicineBefore = new();
            MedicineAfter = new();
        }

        public MedicineStateChange(Medicine medicine, Medicine oldMedicine)
        {
            Id = 0;
            MedicineId = medicine.Id;
            ChangedAt = DateTime.UtcNow;
            MedicineBefore = oldMedicine;
            MedicineAfter = medicine;
        }

        public DateOnly GetDate() => ChangedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (GetDate() != day.Date) return;
            if (!day.MedicineStateChanges.Contains(this)) day.MedicineStateChanges.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            if(MedicineBefore.Active != MedicineAfter.Active)
            {
                list.Add($"{MedicineAfter.Name} active was changed to {MedicineAfter.Active}.");
            }
            if(MedicineBefore.Name != MedicineAfter.Name)
            {
                list.Add($"{MedicineBefore.Name} was changed to {MedicineAfter.Name}.");
            }
            if(MedicineBefore.Dose != MedicineAfter.Dose)
            {
                list.Add($"{MedicineAfter.Name} dose was changed to {MedicineAfter.Dose}.");
            }
            if(MedicineBefore.MedicineSchedule != MedicineAfter.MedicineSchedule)
            {
                list.Add($"{MedicineAfter.Name} schedule was updated.");
            }
            if(MedicineBefore.PrescribedBy != MedicineAfter.PrescribedBy)
            {
                list.Add($"{MedicineAfter.Name} prescribed by was changed to {MedicineAfter.PrescribedBy}.");
            }
            if(MedicineBefore.PrescribedAt != MedicineAfter.PrescribedAt)
            {
                list.Add($"{MedicineAfter.Name} prescribed at was updated to {MedicineAfter.PrescribedAtDate}.");
            }
            return list;
        }
    }
}
