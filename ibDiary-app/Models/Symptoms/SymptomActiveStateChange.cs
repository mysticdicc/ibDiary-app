using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Symptoms
{
    public class SymptomActiveStateChange
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public Symptom Symptom { get; set; }
        public DateTime ChangedAt { get; set; }
        public bool WasActive { get; set; }
        public bool IsActive { get; set; }

        public SymptomActiveStateChange()
        {
            Id = 0;
            Symptom = new();
            ChangedAt = DateTime.UtcNow;
            WasActive = true;
            IsActive = false;
        }

        public SymptomActiveStateChange(Symptom symptom, bool wasActive, bool isActive)
        {
            Id = 0;
            Symptom = symptom;
            ChangedAt = DateTime.UtcNow;
            WasActive = wasActive;
            IsActive = isActive;
        }

        public SymptomActiveStateChange(Symptom oldSymptom, Symptom newSymptom)
        {
            Id = 0;
            Symptom = newSymptom;
            ChangedAt = DateTime.UtcNow;
            WasActive = oldSymptom.Active;
            IsActive = newSymptom.Active;
        }
    }
}
