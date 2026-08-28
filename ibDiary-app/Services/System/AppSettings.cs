using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace ibDiary_app.Services.System
{
    public class AppSettings
    {
        [Setting(true)]
        public bool NotificationsEnabled { get; set; } = true;
        [Setting(true)]
        public bool MedicineReportNotificationsEnabled { get; set; } = true;
        [Setting(true)]
        public bool ScheduledNotificationsEnabled { get; set; } = true;
        [Setting(15)]
        [Range(0, 600, ErrorMessage = "Value must be between 0 and 600 minutes.")]
        public int MinutesBetweenNotifications { get; set; } = 15;
        [Setting(null)]
        public DateTime? NotificationsLastSent { get; set; } = DateTime.MinValue;

        public void Load()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttribute<SettingAttribute>();
                if (attribute == null || !prop.CanWrite) continue;

                if (!Preferences.Default.ContainsKey(prop.Name))
                {
                    if (attribute.DefaultValue != null)
                        prop.SetValue(this, attribute.DefaultValue);
                }
                else
                {
                    string stored = Preferences.Default.Get(prop.Name, string.Empty);
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    if (string.IsNullOrEmpty(stored))
                        prop.SetValue(this, null);
                    else
                    {
                        var value = Convert.ChangeType(stored, targetType);
                        prop.SetValue(this, value);
                    }
                }
            }
        }

        public void Save()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.GetCustomAttribute<SettingAttribute>() == null || !prop.CanRead)
                    continue;

                var value = prop.GetValue(this);
                Preferences.Default.Set(prop.Name, value?.ToString() ?? string.Empty);
            }
        }

        public void Reset()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.GetCustomAttribute<SettingAttribute>() != null)
                    Preferences.Default.Remove(prop.Name);
            }

            Load();
        }

        public bool IsTimeToSendNotification()
        {
            if (NotificationsLastSent == null) return true;
            if (DateTime.UtcNow > NotificationsLastSent.Value.AddMinutes(MinutesBetweenNotifications)) return true;
            return false;
        }
    }
}
