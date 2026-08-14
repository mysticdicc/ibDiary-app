using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineSchedule> MedicineSchedules { get; set; }
        public DbSet<MedicineReport> MedicineReports { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<SymptomReport> SymptomReports { get; set; }
        public DbSet<SymptomActiveStateChange> SymptomStateChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
