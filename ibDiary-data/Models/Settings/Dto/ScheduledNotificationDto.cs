using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ibDiary_data.Models.Settings.Dto
{
    public class ScheduledNotificationDto
    {
        public int Id { get; set; }
        public ScheduledNotificationType Type { get; set; }
        public DateTime StartAtLocal { get; set; }
        public DateTime CreatedAtLocal { get; set; }
        public DateTime LastSentAtLocal { get; set; }

        public DateOnly CreatedAtLocalDate { get => DateOnly.FromDateTime(CreatedAtLocal); }
        public ScheduleIntervalType IntervalType { get; set; }

        [Range(0, 512, ErrorMessage = "Value must be between 0 and 512.")]
        public int IntervalValue { get; set; }

        public bool IsNew { get; set; }
        public bool Active { get; set; }

        public ScheduledNotificationDto()
        {
            Id = 0;
            Type = default;
            StartAtLocal = DateTime.Now;
            CreatedAtLocal = DateTime.Now;
            LastSentAtLocal = DateTime.MinValue;
            IntervalType = ScheduleIntervalType.Days;
            IntervalValue = 1;
            IsNew = true;
            Active = true;
        }
    }
}
