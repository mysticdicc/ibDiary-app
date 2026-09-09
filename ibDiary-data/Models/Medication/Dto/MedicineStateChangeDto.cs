using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineStateChangeDto
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
    }
}
