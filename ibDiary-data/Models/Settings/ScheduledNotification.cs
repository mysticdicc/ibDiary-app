using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ibDiary_data.Models.Settings
{
    public class ScheduledNotification : ICalendarUpdate
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public ScheduledNotificationType Type { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSentAt { get; set; }
        [NotMapped] public DateOnly CreatedAtDate { get => DateOnly.FromDateTime(CreatedAt); }
        public ScheduleIntervalType IntervalType { get; set; }
        [Range(0, 512, ErrorMessage = "Value must be between 0 and 512.")]
        public int IntervalValue { get; set; }
        public bool IsNew { get; set; }
        public bool Active { get; set; }

        public ScheduledNotification(ScheduledNotificationType type)
        {
            Id = 0;
            Type = type;
            StartAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            LastSentAt = DateTime.MinValue;
            IntervalType = ScheduleIntervalType.Days;
            IntervalValue = 1;
            IsNew = true;
            Active = true;
        }

        public void UpdateProperties(ScheduledNotification notification)
        {
            Type = notification.Type;
            StartAt = notification.StartAt;
            IntervalType = notification.IntervalType;
            IntervalValue = notification.IntervalValue;
            Active = notification.Active;
        }

        public DateOnly GetDate() => CreatedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (!day.CreatedNotifications.Contains(this)) day.CreatedNotifications.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            list.Add($"A notification for {Type} was scheduled to repeat every {IntervalValue} {IntervalType}.");
            return list;
        }
    }
}
