using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Generic
{
    public class ConfirmationOption
    {
        public bool Value { get; set; }
        public string DisplayName { get; set; }

        public ConfirmationOption(bool value, string display)
        {
            Value = value;
            DisplayName = display;
        }
    }
}
