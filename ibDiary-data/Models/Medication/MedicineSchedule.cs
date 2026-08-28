using ibDiary_data.Models.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_data.Models.Medication
{
    public class MedicineSchedule
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [JsonIgnore] public Medicine? Medicine { get; set; }
        public MedicineScheduleType Type { get; set; }
        public ScheduleIntervalType IntervalType { get; set; }
        [Range(1, 512, ErrorMessage = "Value must be between 1 and 512.")]
        public int IntervalValue { get; set; }
        [Range(1, 512, ErrorMessage = "Amount must be between 1 and 512.")]
        public int AmountPerDay { get; set; }
        public DateTime StartedAt { get; set; }
        public bool IsNew { get; set; }

        public MedicineSchedule()
        {
            Id = 0;
            IsNew = true;
            StartedAt = DateTime.UtcNow;
            Type = MedicineScheduleType.DailyLimit;
            IntervalType = ScheduleIntervalType.Hours;
            IntervalValue = 1;
            AmountPerDay = 1;
        }

        public void UpdateProperties(MedicineSchedule schedule)
        {
            Medicine = schedule.Medicine;
            Type = schedule.Type;
            IntervalType = schedule.IntervalType;
            IntervalValue = schedule.IntervalValue;
            AmountPerDay = schedule.AmountPerDay;
            StartedAt = schedule.StartedAt;
        }
    }

    public enum MedicineScheduleType
    {
        Interval,
        DailyLimit,
        AsNeeded
    }
}
