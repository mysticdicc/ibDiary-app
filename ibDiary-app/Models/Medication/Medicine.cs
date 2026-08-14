using Android.Animation;
using ibDiary_app.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_app.Models.Medication
{
    public class Medicine
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }
        public string PrescribedBy { get; set; }
        public string Notes { get; set; }
        public DateTime PrescribedAt { get; set; }
        [NotMapped] public virtual DateOnly PrescribedAtDate { get => DateOnly.FromDateTime(PrescribedAt) ; }
        public MedicineSchedule MedicineSchedule { get; set; }
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
            MedicineSchedule = new();
            StateChanges = [];
            IsNew = true;
        }

        public static Medicine FromDbEntry(int id, PropertyValues originalValues)
        {
            var name = (string?)originalValues[nameof(Name)];
            var dose = (string?)originalValues[nameof(Dose)];
            var prescribedBy = (string?)originalValues[nameof(PrescribedBy)];
            var notes = (string?)originalValues[nameof(Notes)];
            var prescribedAt = (DateTime?)originalValues[nameof(PrescribedAt)];
            var active = (bool?)originalValues[nameof(Active)];
            var medicineSchedule = (MedicineSchedule?)originalValues[nameof(MedicineSchedule)];

            return new Medicine
            {
                Id = id,
                Name = name ?? string.Empty,
                Dose = dose ?? string.Empty,
                PrescribedBy = prescribedBy ?? string.Empty,
                Notes = notes ?? string.Empty,
                PrescribedAt = prescribedAt ?? DateTime.UtcNow,
                Active = active ?? true,
                MedicineSchedule = medicineSchedule ?? new()
            };
        }

        public void UpdateProperties(Medicine medicine)
        {
            Name = medicine.Name;
            Dose = medicine.Dose;
            PrescribedBy = medicine.Dose;
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
    }
}
