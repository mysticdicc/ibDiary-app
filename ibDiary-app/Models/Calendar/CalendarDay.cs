using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_app.Models.Calendar
{
    public class CalendarDay
    {
        [Key] public DateOnly Date { get; set; }
        [NotMapped] public string DayOfWeek { get => Date.DayOfWeek.ToString(); }
        [NotMapped] public string Month { get => Date.ToString("MMMM");  }
        [NotMapped] public string Year { get => Date.Year.ToString(); }
        public List<MedicineReport> MedicineReports { get; set; }
        public List<MedicineStateChange> MedicineStateChanges { get; set; }
        public List<SymptomReport> SymptomReports { get; set; }
        public List<SymptomStateChange> SymptomStateChanges { get; set; }
        public bool IsNew { get; set; }

        public CalendarDay()
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
            MedicineReports = [];
            MedicineStateChanges = [];
            SymptomReports = [];
            SymptomStateChanges = [];
            IsNew = true;
        }

        public CalendarDay(DateOnly date)
        {
            Date = date;
            MedicineReports = [];
            MedicineStateChanges = [];
            SymptomReports = [];
            SymptomStateChanges = [];
            IsNew = true;
        }

        public void UpdateProperties(CalendarDay day)
        {
            if (day.Date != Date) return;
            MedicineReports = day.MedicineReports;
            MedicineStateChanges = day.MedicineStateChanges;
            SymptomReports = day.SymptomReports;
            SymptomStateChanges = day.SymptomStateChanges;
        }

        public bool HasChangedState(CalendarDay old)
        {
            return
                MedicineReports != old.MedicineReports ||
                MedicineStateChanges != old.MedicineStateChanges ||
                SymptomReports != old.SymptomReports ||
                SymptomStateChanges != old.SymptomStateChanges;
        }

        public CalendarDay Clone()
        {
            var clone = new CalendarDay();

            foreach (var property in typeof(CalendarDay).GetProperties())
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
