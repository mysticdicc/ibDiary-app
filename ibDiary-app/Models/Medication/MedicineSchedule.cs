using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_app.Models.Medication
{
    public class MedicineSchedule
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [JsonIgnore] public Medicine? Medicine { get; set; }
        public MedicineScheduleType Type { get; set; }
        public MedicineScheduleIntervalType IntervalType { get; set; }
        public int IntervalValue { get; set; }
        public DateTime StartedAt { get; set; }
        public bool IsNew { get; set; }

        public MedicineSchedule()
        {
            Id = 0;
            IsNew = true;
        }

        public void UpdateProperties(MedicineSchedule schedule)
        {
            Medicine = schedule.Medicine;
            Type = schedule.Type;
            IntervalType = schedule.IntervalType;
            IntervalValue = schedule.IntervalValue;
        }
    }

    public enum MedicineScheduleIntervalType
    {
        Minutes,
        Hours,
        Days,
        Months,
        PerDay
    }

    public enum MedicineScheduleType
    {
        Interval,
        DailyLimit,
        AsNeeded
    }
}
