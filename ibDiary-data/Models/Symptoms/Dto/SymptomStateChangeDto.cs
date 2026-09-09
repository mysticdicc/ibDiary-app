using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Symptoms.Dto
{
    public class SymptomStateChangeDto
    {
        public int Id { get; set; }
        public int SymptomId { get; set; }
        public DateTime ChangedAtLocal { get; set; }
        public DateOnly ChangedAtLocalDate { get => DateOnly.FromDateTime(ChangedAtLocal); }
        public SymptomDto SymptomBefore { get; set; }
        public SymptomDto SymptomAfter { get; set; }
        public bool IsNew { get; set; }

        public SymptomStateChangeDto()
        {
            Id = 0;
            SymptomId = 0;
            ChangedAtLocal = DateTime.Now;
            SymptomBefore = new();
            SymptomAfter = new();
            IsNew = true;
        }
    }
}
