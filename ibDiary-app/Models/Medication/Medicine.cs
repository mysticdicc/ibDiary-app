using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
            IsNew = true;
        }
    }
}
