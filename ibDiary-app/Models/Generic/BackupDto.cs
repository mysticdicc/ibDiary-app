using ibDiary_app.Models.Calendar;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Generic
{
    public class BackupDto
    {
        public List<Medicine> Medicines { get; set; }
        public List<MedicineStateChange> MedicineStateChanges { get; set; }
        public List<MedicineReport> MedicineReports { get; set; }
        public List<Symptom> Symptoms { get; set; }
        public List<SymptomStateChange> SymptomStateChanges { get; set; }
        public List<SymptomReport> SymptomReports { get; set; }
        public List<CalendarDay> CalendarDays { get; set; }

        public BackupDto()
        {
            Medicines = [];
            MedicineStateChanges = [];
            MedicineReports = [];
            Symptoms = [];
            SymptomStateChanges = [];
            SymptomReports = [];
            CalendarDays = [];
        }
    }
}
