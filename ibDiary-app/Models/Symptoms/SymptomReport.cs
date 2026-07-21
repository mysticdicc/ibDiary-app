using System;
using System.Collections.Generic;
using System.Text;
using ibDiary_app.Models.Medication;
using SQLite;

namespace ibDiary_app.Models.Symptoms
{
    public class SymptomReport
    {
        [PrimaryKey]
        public int Id { get; set; }
        public Symptom Symptom { get; set; }
        public Medicine? Medication { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime SubmittedFor { get; set; }
        public int Severity { get; set; }

        public SymptomReport()
        {
            Id = 0;
            Symptom = new();
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Severity = 0;
        }
    }
}
