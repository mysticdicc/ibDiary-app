using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Symptoms.Dto
{
    public class SymptomStateChangeDto : ICalendarUpdate
    {
        public int Id { get; set; }
        public int SymptomId { get; set; }
        public DateTime ChangedAtLocal { get; set; }
        public DateOnly ChangedAtLocalDate { get => DateOnly.FromDateTime(ChangedAtLocal); }
        public SymptomDto SymptomBefore { get; set; }
        public SymptomDto SymptomAfter { get; set; }
        public bool IsNew { get; set; }

        public SymptomStateChangeDto()
        {
            Id = 0;
            SymptomId = 0;
            ChangedAtLocal = DateTime.Now;
            SymptomBefore = new();
            SymptomAfter = new();
            IsNew = true;
        }

        public DateOnly GetDate() => ChangedAtLocalDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            return;
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
