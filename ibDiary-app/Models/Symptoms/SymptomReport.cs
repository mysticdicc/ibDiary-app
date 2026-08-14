using ibDiary_app.Models.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Symptoms
{
    public class SymptomReport
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public Symptom Symptom { get; set; }
        public Medicine? Medication { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime SubmittedFor { get; set; }
        public int Severity { get; set; }
        public bool IsNew { get; set; }

        public SymptomReport()
        {
            Id = 0;
            Symptom = new();
            SubmittedAt = DateTime.UtcNow;
            SubmittedFor = SubmittedAt;
            Severity = 0;
            IsNew = true;
        }
    }
}
