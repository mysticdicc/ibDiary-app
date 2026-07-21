using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Medication
{
    public class MedicineReport
    {
        [PrimaryKey]
        public int Id { get; set; }
        public Medicine Medicine { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime SubmittedFor { get; set; }
        public string Notes { get; set; }

        public MedicineReport()
        {
            Id = 0;
            Medicine = new();
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Notes = string.Empty;
        }
    }
}
