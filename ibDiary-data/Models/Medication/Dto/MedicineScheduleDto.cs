using ibDiary_data.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Medication.Dto
{
    public class MedicineScheduleDto
    {
        public int Id { get; set; }

        public MedicineScheduleType Type { get; set; }
        public ScheduleIntervalType IntervalType { get; set; }

        [Range(1, 512, ErrorMessage = "Value must be between 1 and 512.")]
        public int IntervalValue { get; set; }

        [Range(1, 512, ErrorMessage = "Amount must be between 1 and 512.")]
        public int AmountPerDay { get; set; }

        public DateTime StartedAtLocal { get; set; }
        public DateOnly StartedAtLocalDate { get => DateOnly.FromDateTime(StartedAtLocal); }
        public bool IsNew { get; set; }

        public MedicineScheduleDto()
        {
            Id = 0;
            Type = MedicineScheduleType.DailyLimit;
            IntervalType = ScheduleIntervalType.Hours;
            IntervalValue = 1;
            AmountPerDay = 1;
            StartedAtLocal = DateTime.Now;
            IsNew = true;
        }
    }
}
