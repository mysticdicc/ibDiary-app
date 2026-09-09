using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineDueAtOccuranceDto
    {
        public int Id { get; set; }
        public MedicineDto Medicine { get; set; }
        public MedicineDueAtStatus Status { get; set; }
        public DateTime DueAtLocal { get; set; }
        public DateTime CreatedAtLocal { get; set; }

        public MedicineDueAtOccuranceDto()
        {
            Id = 0;
            Medicine = new();
            Status = MedicineDueAtStatus.Pending;
            DueAtLocal = DateTime.Now;
            CreatedAtLocal = DateTime.Now;
        }

        public MedicineDueAtOccuranceDto(MedicineDto medicine)
        {
            Id = 0;
            Medicine = medicine;
            Status = MedicineDueAtStatus.Pending;
            DueAtLocal = DateTime.Now;
            CreatedAtLocal = DateTime.Now;
        }
    }
}
