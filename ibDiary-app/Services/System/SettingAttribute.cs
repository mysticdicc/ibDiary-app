using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.System
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
