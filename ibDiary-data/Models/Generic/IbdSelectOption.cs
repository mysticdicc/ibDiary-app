using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Generic
{
    public class IbdSelectOption
    {
        public string DisplayName { get; set; }
        public object? Value { get; set; }

        public IbdSelectOption()
        {
            DisplayName = string.Empty;
        }
        
        public IbdSelectOption(string display, object? value)
        {
            DisplayName = display;
            Value = value;
        }
    }
}
