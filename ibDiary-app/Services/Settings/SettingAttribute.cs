using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        public object? DefaultValue { get; set; }

        public SettingAttribute(object? defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
