using ibDiary_data.Models.Calendar;
using ibDiary_data.Models.Interfaces;
using ibDiary_data.Models.Settings;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ibDiary_data.Models.Medication
{
    public class Medicine : ICalendarUpdate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Name must not exceed 128 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Dose cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Dose must not exceed 128 characters.")]
        public string Dose { get; set; }
        [Required(ErrorMessage = "Prescribed by cannot be empty.")]
        [MaxLength(128, ErrorMessage = "Prescribed by must not exceed 128 characters.")]
        public string PrescribedBy { get; set; }
        [MaxLength(1024, ErrorMessage = "Notes must not exceed 1024 characters.")]
        public string Notes { get; set; }
        public DateTime PrescribedAt { get; set; }
        [NotMapped] 
        public virtual DateOnly PrescribedAtDate { get => DateOnly.FromDateTime(PrescribedAt) ; }
        public int MedicineScheduleId { get; set; }
        public MedicineSchedule MedicineSchedule { get; set; }
        [JsonIgnore]
        [NotMapped] 
        public List<MedicineStateChange> StateChanges { get; set; }
        [JsonIgnore]
        public List<MedicineDueAtOccurance> MedicineOccurances { get; set; }
        public bool Active { get; set; }
        public bool IsNew { get; set; }
        public List<MedicineReport> MedicineReports { get; set; }

        public Medicine()
        {
            Id = 0;
            Name = string.Empty;
            Dose = string.Empty;
            PrescribedBy = string.Empty;
            Notes = string.Empty;
            PrescribedAt = DateTime.UtcNow;
            Active = true;
            StateChanges = [];
            MedicineOccurances = [];
            IsNew = true;
            MedicineSchedule = new();
            MedicineReports = [];
        }

        public void RegenerateOccurances(DateTime upToUtc)
        {
            if (MedicineSchedule.Type == MedicineScheduleType.AsNeeded)
            {
                MedicineOccurances.Clear();
                return;
            }

            var existingDueSet = MedicineOccurances
                .Select(x => DateTime.SpecifyKind(x.DueAt, DateTimeKind.Utc))
                .ToHashSet();

            foreach (var dueAt in EnumerateDueTimes(upToUtc))
            {
                if (existingDueSet.Contains(dueAt)) continue;

                var occ = new MedicineDueAtOccurance(dueAt, this);
                MedicineOccurances.Add(occ);
                existingDueSet.Add(dueAt);
            }
        }

        private IEnumerable<DateTime> EnumerateDueTimes(DateTime upToUtc)
        {
            var startUtc = DateTime.SpecifyKind(MedicineSchedule.StartedAt, DateTimeKind.Utc);

            if (startUtc > upToUtc)
                yield break;

            if (MedicineSchedule.Type == MedicineScheduleType.Interval)
            {
                var cursor = startUtc;
                while (cursor <= upToUtc)
                {
                    yield return cursor;
                    cursor = AddInterval(cursor, MedicineSchedule.IntervalType, MedicineSchedule.IntervalValue);
                }

                yield break;
            }

            var day = DateOnly.FromDateTime(startUtc);
            var endDay = DateOnly.FromDateTime(upToUtc);
            var startTime = TimeOnly.FromDateTime(startUtc);

            while (day <= endDay)
            {
                var doseTime = day.ToDateTime(startTime, DateTimeKind.Utc);

                for (var i = 0; i < MedicineSchedule.AmountPerDay; i++)
                {
                    if (doseTime >= startUtc && doseTime <= upToUtc)
                        yield return doseTime;

                    doseTime = AddInterval(doseTime, MedicineSchedule.IntervalType, MedicineSchedule.IntervalValue);
                }

                day = day.AddDays(1);
            }
        }

        private static DateTime AddInterval(DateTime value, ScheduleIntervalType type, int intervalValue)
        {
            return type switch
            {
                ScheduleIntervalType.Minutes => value.AddMinutes(intervalValue),
                ScheduleIntervalType.Hours => value.AddHours(intervalValue),
                ScheduleIntervalType.Days => value.AddDays(intervalValue),
                ScheduleIntervalType.Months => value.AddMonths(intervalValue),
                _ => value
            };
        }

        public static Medicine FromDbEntry(int id, PropertyValues originalValues)
        {
            var name = (string?)originalValues[nameof(Name)];
            var dose = (string?)originalValues[nameof(Dose)];
            var prescribedBy = (string?)originalValues[nameof(PrescribedBy)];
            var notes = (string?)originalValues[nameof(Notes)];
            var prescribedAt = (DateTime?)originalValues[nameof(PrescribedAt)];
            var active = (bool?)originalValues[nameof(Active)];

            return new Medicine
            {
                Id = id,
                Name = name ?? string.Empty,
                Dose = dose ?? string.Empty,
                PrescribedBy = prescribedBy ?? string.Empty,
                Notes = notes ?? string.Empty,
                PrescribedAt = prescribedAt ?? DateTime.UtcNow,
                Active = active ?? true,
                MedicineSchedule = new()
            };
        }

        public void UpdateProperties(Medicine medicine)
        {
            Name = medicine.Name;
            Dose = medicine.Dose;
            PrescribedBy = medicine.PrescribedBy;
            Notes = medicine.Notes;
            PrescribedAt = medicine.PrescribedAt;
            Active = medicine.Active;

            MedicineSchedule.UpdateProperties(medicine.MedicineSchedule);

            UpdateOccurances(medicine.MedicineOccurances);
            UpdateReports(medicine.MedicineReports);
        }

        private void UpdateOccurances(List<MedicineDueAtOccurance> incoming)
        {
            var existingById = MedicineOccurances.ToDictionary(x => x.Id);
            var merged = new List<MedicineDueAtOccurance>();

            foreach (var src in incoming)
            {
                if (src.Id > 0 && existingById.TryGetValue(src.Id, out var tracked))
                {
                    tracked.Status = src.Status;
                    tracked.DueAt = src.DueAt;
                    tracked.CreatedAt = src.CreatedAt;
                    tracked.Medicine = this;
                    merged.Add(tracked);
                    continue;
                }

                var created = new MedicineDueAtOccurance
                {
                    Id = src.Id,
                    Status = src.Status,
                    DueAt = src.DueAt,
                    CreatedAt = src.CreatedAt,
                    Medicine = this
                };

                merged.Add(created);
            }

            MedicineOccurances = merged;
        }

        private void UpdateReports(List<MedicineReport> incoming)
        {
            var existingById = MedicineReports.ToDictionary(x => x.Id);
            var occById = MedicineOccurances.Where(x => x.Id > 0).ToDictionary(x => x.Id);
            var merged = new List<MedicineReport>();

            foreach (var src in incoming)
            {
                var dueAt = (src.DueAt?.Id > 0 && occById.TryGetValue(src.DueAt.Id, out var trackedOcc))
                    ? trackedOcc
                    : src.DueAt;

                if (src.Id > 0 && existingById.TryGetValue(src.Id, out var tracked))
                {
                    tracked.Medicine = this;
                    tracked.MedicineId = Id;
                    tracked.SubmittedAt = src.SubmittedAt;
                    tracked.MedicineTakenAt = src.MedicineTakenAt;
                    tracked.MedicineTaken = src.MedicineTaken;
                    tracked.Dose = src.Dose;
                    tracked.Notes = src.Notes;
                    tracked.DueAt = dueAt;
                    merged.Add(tracked);
                    continue;
                }

                var created = new MedicineReport
                {
                    Id = src.Id,
                    Medicine = this,
                    MedicineId = Id,
                    SubmittedAt = src.SubmittedAt,
                    MedicineTakenAt = src.MedicineTakenAt,
                    MedicineTaken = src.MedicineTaken,
                    Dose = src.Dose,
                    Notes = src.Notes,
                    DueAt = dueAt
                };

                merged.Add(created);
            }

            MedicineReports = merged;
        }

        public bool HasChangedState(Medicine medicine)
        {
            var scheduleChanged =
                MedicineSchedule.Type != medicine.MedicineSchedule.Type ||
                MedicineSchedule.IntervalType != medicine.MedicineSchedule.IntervalType ||
                MedicineSchedule.IntervalValue != medicine.MedicineSchedule.IntervalValue ||
                MedicineSchedule.AmountPerDay != medicine.MedicineSchedule.AmountPerDay ||
                MedicineSchedule.StartedAt != medicine.MedicineSchedule.StartedAt;

            var thisOcc = MedicineOccurances
                .OrderBy(x => x.DueAt)
                .ThenBy(x => x.Id)
                .Select(x => new { x.Id, x.DueAt, x.Status });

            var otherOcc = medicine.MedicineOccurances
                .OrderBy(x => x.DueAt)
                .ThenBy(x => x.Id)
                .Select(x => new { x.Id, x.DueAt, x.Status });

            var occChanged = !thisOcc.SequenceEqual(otherOcc);

            return
                Name != medicine.Name ||
                Dose != medicine.Dose ||
                PrescribedBy != medicine.PrescribedBy ||
                Notes != medicine.Notes ||
                PrescribedAt != medicine.PrescribedAt ||
                Active != medicine.Active ||
                scheduleChanged ||
                occChanged;
        }

        public Medicine Clone()
        {
            var clone = new Medicine();

            foreach (var property in typeof(Medicine).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(clone, property.GetValue(this));
                }
            }

            clone.MedicineOccurances = MedicineOccurances
                .Select(o =>
                {
                    var c = o.Clone();
                    c.Medicine = clone;
                    return c;
                })
                .ToList();

            return clone;
        }

        public DateOnly GetDate() => PrescribedAtDate;

        public void AddToCalendarDay(CalendarDay day)
        {
            if (GetDate() != day.Date) return;
            if (!day.CreatedMedicines.Contains(this)) day.CreatedMedicines.Add(this);
        }

        public List<string> GetCalendarUpdate()
        {
            var list = new List<string>();
            list.Add($"You were prescribed {Dose} of {Name} by {PrescribedBy}.");
            return list;
        }
    }
}
