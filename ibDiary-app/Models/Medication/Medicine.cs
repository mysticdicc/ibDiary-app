using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Medication
{
    public class Medicine
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }
        public string PrescribedBy { get; set; }
        public string Notes { get; set; }
        public DateOnly PrescribedAt { get; set; }
        public MedicineSchedule MedicineSchedule { get; set; }
        public bool Active { get; set; }

        public Medicine()
        {
            Id = 0;
            Name = string.Empty;
            Dose = string.Empty;
            PrescribedBy = string.Empty;
            Notes = string.Empty;
            PrescribedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Active = true;
            MedicineSchedule = new();
        }
    }
}
