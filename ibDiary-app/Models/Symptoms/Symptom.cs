using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_app.Models.Symptoms
{
    public class Symptom : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool IsNew { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        [NotMapped][JsonIgnore] public List<SymptomStateChange> StateChanges { get; set; }
        public Symptom()
        {
            Id = 0;
            Title = string.Empty;
            Description = string.Empty;
            Active = true;
            IsNew = true;
            StateChanges = [];
            CreatedAt = DateTime.UtcNow;
            StartedAt = CreatedAt;
        }

        public static Symptom FromDbEntry(int id, PropertyValues originalValues)
        {
            var title = (string?)originalValues[nameof(Title)];
            var description = (string?)originalValues[nameof(Description)];
            var active = (bool?)originalValues[nameof(Active)];

            return new Symptom
            {
                Id = id,
                Title = title ?? string.Empty,
                Description = description ?? string.Empty,
                Active = active ?? true,
                IsNew = true
            };
        }

        public void UpdateProperties(Symptom symptom)
        {
            Title = symptom.Title;
            Description = symptom.Description;
            Active = symptom.Active;
        }

        public bool HasChangedState(Symptom symptom)
        {
            var titleChanged = Title != symptom.Title;
            var descChanged = Description != symptom.Description;
            var activeChanged = Active != symptom.Active;

            return titleChanged || descChanged || activeChanged;
        }

        public Symptom Clone()
        {
            var clone = new Symptom();

            foreach (var property in typeof(Symptom).GetProperties())
            {
                if (property.Name == nameof(StateChanges) || property.Name == nameof(IsNew))
                    continue;

                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            return clone;
        }

        public DateOnly GetDate() => CreatedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (GetDate() != day.Date) return;
            if (!day.CreatedSymptoms.Contains(this)) day.CreatedSymptoms.Add(this);
        }
    }
}
