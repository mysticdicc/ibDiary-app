using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_data.Models.Medication
{
    public class Medicine : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
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
        public DateTime PrescribedAt { get; set; }
        [NotMapped] public virtual DateOnly PrescribedAtDate { get => DateOnly.FromDateTime(PrescribedAt) ; }
        public int MedicineScheduleId { get; set; }
        virtual public MedicineSchedule MedicineSchedule { get; set; }
        [JsonIgnore][NotMapped] public List<MedicineStateChange> StateChanges { get; set; }
        public bool Active { get; set; }
        public bool IsNew { get; set; }

        public Medicine()
        {
            Id = 0;
            Name = string.Empty;
            Dose = string.Empty;
            PrescribedBy = string.Empty;
            Notes = string.Empty;
            PrescribedAt = DateTime.UtcNow;
            Active = true;
            StateChanges = [];
            IsNew = true;
            MedicineSchedule = new();
        }

        public static Medicine FromDbEntry(int id, PropertyValues originalValues)
        {
            var name = (string?)originalValues[nameof(Name)];
            var dose = (string?)originalValues[nameof(Dose)];
            var prescribedBy = (string?)originalValues[nameof(PrescribedBy)];
            var notes = (string?)originalValues[nameof(Notes)];
            var prescribedAt = (DateTime?)originalValues[nameof(PrescribedAt)];
            var active = (bool?)originalValues[nameof(Active)];

            return new Medicine
            {
                Id = id,
                Name = name ?? string.Empty,
                Dose = dose ?? string.Empty,
                PrescribedBy = prescribedBy ?? string.Empty,
                Notes = notes ?? string.Empty,
                PrescribedAt = prescribedAt ?? DateTime.UtcNow,
                Active = active ?? true,
                MedicineSchedule = new()
            };
        }

        public void UpdateProperties(Medicine medicine)
        {
            Name = medicine.Name;
            Dose = medicine.Dose;
            PrescribedBy = medicine.PrescribedBy;
            Notes = medicine.Notes;
            PrescribedAt = medicine.PrescribedAt;
            Active = medicine.Active;
            MedicineSchedule = medicine.MedicineSchedule;
        }

        public bool HasChangedState(Medicine medicine)
        {
            return
                Name != medicine.Name ||
                Dose != medicine.Dose ||
                PrescribedBy != medicine.PrescribedBy ||
                Notes != medicine.Notes ||
                PrescribedAt != medicine.PrescribedAt ||
                Active != medicine.Active ||
                MedicineSchedule != medicine.MedicineSchedule;
        }

        public Medicine Clone()
        {
            var clone = new Medicine();

            foreach (var property in typeof(Medicine).GetProperties())
            {
                if (property.Name == nameof(StateChanges) || property.Name == nameof(IsNew))
                    continue;

                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            return clone;
        }

        public DateOnly GetDate() => PrescribedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (GetDate() != day.Date) return;
            if (!day.CreatedMedicines.Contains(this)) day.CreatedMedicines.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            list.Add($"You were prescribed {Dose} of {Name} by {PrescribedBy}.");
            return list;
        }
    }
}
