using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Food;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Settings;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ibDiary_data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineSchedule> MedicineSchedules { get; set; }
        public DbSet<MedicineReport> MedicineReports { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<SymptomReport> SymptomReports { get; set; }
        public DbSet<SymptomStateChange> SymptomStateChanges { get; set; }
        public DbSet<MedicineStateChange> MedicineStateChanges { get; set; }
        public DbSet<CalendarDay> CalendarDays { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealReport> MealReports { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodItemReport> FoodReports { get; set; }
        public DbSet<ScheduledNotification> ScheduledNotifications { get; set; }
        public DbSet<MedicineDueAtOccurance> MedicineOccurances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };

            var medicineConverter = new ValueConverter<Medicine, string>(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<Medicine>(v, options) ?? new Medicine()
            );

            var symptomConverter = new ValueConverter<Symptom, string>(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<Symptom>(v, options) ?? new Symptom()
            );

            modelBuilder.Entity<MedicineStateChange>()
                .HasOne(m => m.Medicine)
                .WithMany(m => m.StateChanges)
                .HasForeignKey(m => m.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicineStateChange>()
                .Property(m => m.MedicineBefore)
                .HasColumnType("jsonb")
                .HasConversion(medicineConverter);

            modelBuilder.Entity<MedicineStateChange>()
                .Property(m => m.MedicineAfter)
                .HasColumnType("jsonb")
                .HasConversion(medicineConverter);

            modelBuilder.Entity<SymptomStateChange>()
                .HasOne(s => s.Symptom)
                .WithMany(s => s.StateChanges)
                .HasForeignKey(s => s.SymptomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SymptomStateChange>()
                .Property(s => s.SymptomBefore)
                .HasColumnType("jsonb")
                .HasConversion(symptomConverter);

            modelBuilder.Entity<SymptomStateChange>()
                .Property(s => s.SymptomAfter)
                .HasColumnType("jsonb")
                .HasConversion(symptomConverter);

            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.MedicineSchedule)
                .WithOne(ms => ms.Medicine)
                .HasForeignKey<Medicine>(m => m.MedicineScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
