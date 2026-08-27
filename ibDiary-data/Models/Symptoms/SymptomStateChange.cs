using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Medication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Symptoms
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
            if (GetDate() != day.Date) return;
            if (!day.SymptomStateChanges.Contains(this)) day.SymptomStateChanges.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            if (SymptomBefore.Active != SymptomAfter.Active)
            {
                list.Add($"{SymptomAfter.Title} active was changed to {SymptomAfter.Active}.");
            }
            if (SymptomBefore.Description != SymptomAfter.Description)
            {
                list.Add($"{SymptomAfter.Title} description was updated.");
            }
            if (SymptomBefore.Title != SymptomAfter.Title)
            {
                list.Add($"{SymptomBefore.Title} name was changed to {SymptomAfter.Title}.");
            }

            return list;
        }
    }
}
