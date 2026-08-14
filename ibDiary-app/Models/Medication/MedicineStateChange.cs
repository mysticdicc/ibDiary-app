using ibDiary_app.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Medication
{
    public class MedicineStateChange
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
    }
}
