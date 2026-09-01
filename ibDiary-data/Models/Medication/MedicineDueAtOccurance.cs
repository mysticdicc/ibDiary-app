using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Medication
{
    public class MedicineDueAtOccurance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Medicine Medicine { get; set; }
        public MedicineDueAtStatus Status { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public MedicineDueAtOccurance()
        {
            Id = 0;
            Medicine = new();
            Status = MedicineDueAtStatus.Pending;
            DueAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
        }

        public MedicineDueAtOccurance(DateTime dueAt, Medicine medicine)
        {
            Id = 0;
            Medicine = medicine;
            Status = MedicineDueAtStatus.Pending;
            DueAt = dueAt;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateProperties(MedicineDueAtOccurance occ)
        {
            Medicine = occ.Medicine;
            Status = occ.Status;
            DueAt = occ.DueAt;
        }

        public MedicineDueAtOccurance Clone()
        {
            var clone = new MedicineDueAtOccurance();

            foreach (var property in typeof(MedicineDueAtOccurance).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            return clone;
        }
    }
}
