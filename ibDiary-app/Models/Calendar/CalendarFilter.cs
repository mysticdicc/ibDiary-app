using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Calendar
{
    public class CalendarFilter
    {
        public bool DateFilterEnabled { get; set; }
        public DateOnly DateFilterFrom { get; set; }
        public DateOnly DateFilterTo { get; set; }
        public bool MedicineFilterEnabled { get; set; }
        public Medicine Medicine { get; set; }
        public bool SymptomFilterEnabled { get; set; }
        public Symptom Symptom { get; set; }
        public bool ShowInactive { get; set; }
        public bool ShowMedicineReports { get; set; }
        public bool ShowSymptomReports { get; set; }
        public bool ShowCreatedMedicines { get; set; }
        public bool ShowAddedSymptoms { get; set; }

        public CalendarFilter()
        {
            DateFilterEnabled = false;
            DateFilterFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-365));
            DateFilterTo = DateOnly.FromDateTime(DateTime.UtcNow);
            MedicineFilterEnabled = false;
            Medicine = new();
            SymptomFilterEnabled = false;
            Symptom = new();
            ShowInactive = true;
            ShowMedicineReports = true;
            ShowSymptomReports = true;
            ShowCreatedMedicines = true;
            ShowAddedSymptoms = true;
        }

        public void ChangeSymptomFilter(bool enabled, Symptom? symptom)
        {
            if (enabled)
            {
                if (null == symptom) return;
                SymptomFilterEnabled = true;
                Symptom = symptom;
            }
            else
            {
                SymptomFilterEnabled = false;
                Symptom = new();
            }
        }

        public void ChangeMedicineFilter(bool enabled, Medicine? medicine)
        {
            if (enabled)
            {
                if (null == medicine) return;
                MedicineFilterEnabled = true;
                Medicine = medicine;
            }
            else
            {
                MedicineFilterEnabled = false;
                Medicine = new();
            }
        }
    }
}
