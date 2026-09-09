using ibDiary_app.Services.Settings;
using System;

namespace ibDiary_app.Services.System
{
    public class DateLocalisationService(AppSettings settings)
    {
        private readonly AppSettings _settings = settings;

        private TimeZoneInfo GetTimeZone()
        {
            if (!string.IsNullOrWhiteSpace(_settings.TimeZoneId))
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(_settings.TimeZoneId);
                }
                catch
                {
                }
            }

            return TimeZoneInfo.Local;
        }

        public DateTime UtcToLocalTime(DateTime utcTime)
        {
            if (utcTime == DateTime.MinValue) return DateTime.MinValue;
            if (utcTime.Kind == DateTimeKind.Local) return utcTime;

            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcTime, DateTimeKind.Utc), GetTimeZone());
        }

        public DateTime LocalToUtcTime(DateTime localTime)
        {
            if (localTime == DateTime.MinValue) return DateTime.MinValue;
            if (localTime.Kind == DateTimeKind.Utc) return localTime;

            var unspecified = DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecified, GetTimeZone());
        }

        public string UtcToLocalTimeString(DateTime utcTime)
        {
            return UtcToLocalTime(utcTime).ToLongDateString();
        }

        public DateTime GetCurrentLocalTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, GetTimeZone());
        }
    }
}