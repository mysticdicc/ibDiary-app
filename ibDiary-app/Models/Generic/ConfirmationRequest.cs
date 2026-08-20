using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Generic
{
    public class ConfirmationRequest
    {
        public string Message { get; set; } = string.Empty;
        public ConfirmationOption TrueOption = new(true, "");
        public ConfirmationOption FalseOption = new(false, "");
        public bool Result { get; set; }
    }
}
