using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ibDiary_app.Models.Settings
{
    public class AppSettings
    {
        [Setting(true)]
        public bool NotificationsEnabled { get; set; } = true;
        [Setting(true)]
        public bool MedicineReportNotificationsEnabled { get; set; } = true;

        public void Load()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttribute<SettingAttribute>();
                if (attribute == null || !prop.CanWrite)
                    continue;

                if (!Preferences.Default.ContainsKey(prop.Name))
                {
                    if (attribute.DefaultValue != null)
                        prop.SetValue(this, attribute.DefaultValue);
                }
                else
                {
                    string stored = Preferences.Default.Get(prop.Name, string.Empty);
                    var value = Convert.ChangeType(stored, prop.PropertyType);
                    prop.SetValue(this, value);
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
    }
}
