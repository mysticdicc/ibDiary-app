using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Models.Symptoms
{
    public class Symptom
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public Symptom()
        {
            Id = 0;
            Title = string.Empty;
            Description = string.Empty;
            Active = true;
        }
    }
}
