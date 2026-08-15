using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using ibDiary_app.Models.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Symptoms
{
    public class SymptomStateChange : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [ForeignKey(nameof(SymptomId))] public Symptom? Symptom { get; set; }
        public int SymptomId { get; set; }
        public DateTime ChangedAt { get; set; }
        [NotMapped] public DateOnly ChangedAtDate { get => DateOnly.FromDateTime(ChangedAt); }
        [Column(TypeName = "jsonb")] public Symptom SymptomBefore { get; set; }
        [Column(TypeName = "jsonb")] public Symptom SymptomAfter { get; set; }

        public SymptomStateChange()
        {
            Id = 0;
            SymptomId = 0;
            ChangedAt = DateTime.UtcNow;
            SymptomBefore = new();
            SymptomAfter = new();
        }

        public SymptomStateChange(Symptom symptom, Symptom oldSymptom)
        {
            Id = 0;
            SymptomId = symptom.Id;
            ChangedAt = DateTime.UtcNow;
            SymptomBefore = oldSymptom;
            SymptomAfter = symptom;
        }

        public DateOnly GetDate() => ChangedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.SymptomStateChanges.Contains(this)) day.SymptomStateChanges.Add(this);
        }
    }
}
