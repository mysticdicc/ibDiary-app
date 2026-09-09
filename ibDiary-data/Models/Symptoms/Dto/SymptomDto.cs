using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_data.Models.Symptoms.Dto
{
    public class SymptomDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Name must not be longer than 128 characters.")]
        public string Title { get; set; }
        [MaxLength(1024, ErrorMessage = "Description must not be longer than 1024 characters.")]
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool IsNew { get; set; }
        public DateTime CreatedAtLocal { get; set; }
        public DateTime StartedAtLocal { get; set; }
        public DateOnly CreatedAtLocalDate { get => DateOnly.FromDateTime(CreatedAtLocal); }
        public List<SymptomStateChangeDto> StateChanges { get; set; }
        public List<SymptomReportDto> SymptomReports { get; set; }

        public SymptomDto()
        {
            Id = 0;
            Title = string.Empty;
            Description = string.Empty;
            Active = true;
            IsNew = true;
            CreatedAtLocal = DateTime.Now;
            StartedAtLocal = CreatedAtLocal;
            StateChanges = [];
            SymptomReports = [];
        }
    }
}
