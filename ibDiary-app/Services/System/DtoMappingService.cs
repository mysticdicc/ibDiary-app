using ibDiary_data.Models.Food;
using ibDiary_data.Models.Food.Dto;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Medication.Dto;
using ibDiary_data.Models.Settings;
using ibDiary_data.Models.Settings.Dto;
using ibDiary_data.Models.Symptoms;
using ibDiary_data.Models.Symptoms.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ibDiary_app.Services.System
{
    public class DtoMappingService(DateLocalisationService dateService)
    {
        private readonly DateLocalisationService _dateService = dateService;

        private DateTime ToLocal(DateTime utc)
        {
            if (utc == DateTime.MinValue)
                return DateTime.MinValue;

            return _dateService.UtcToLocalTime(utc);
        }

        private DateTime ToUtc(DateTime local)
        {
            if (local == DateTime.MinValue)
                return DateTime.MinValue;

            return _dateService.LocalToUtcTime(local);
        }

        private static List<TDto> MapList<TModel, TDto>(IEnumerable<TModel>? source, Func<TModel, TDto> map)
        {
            return source?.Select(map).ToList() ?? [];
        }

        public SymptomDto ToDto(Symptom model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            Active = model.Active,
            IsNew = model.IsNew,
            CreatedAtLocal = ToLocal(model.CreatedAtUtc),
            StartedAtLocal = ToLocal(model.StartedAtUtc),
            StateChanges = MapList(model.StateChanges, ToDto),
            SymptomReports = MapList(model.SymptomReports, ToDto)
        };

        public Symptom FromDto(SymptomDto dto) => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Active = dto.Active,
            IsNew = dto.IsNew,
            CreatedAtUtc = ToUtc(dto.CreatedAtLocal),
            StartedAtUtc = ToUtc(dto.StartedAtLocal),
            StateChanges = MapList(dto.StateChanges, FromDto),
            SymptomReports = MapList(dto.SymptomReports, FromDto)
        };

        public SymptomReportDto ToDto(SymptomReport model) => new()
        {
            Id = model.Id,
            Symptom = ToDto(model.Symptom),
            Medication = model.Medication is null ? null : ToDto(model.Medication),
            SubmittedAtLocal = ToLocal(model.SubmittedAt),
            SubmittedForLocal = ToLocal(model.SubmittedFor),
            Severity = model.Severity,
            Notes = model.Notes,
            IsNew = model.IsNew
        };

        public SymptomReport FromDto(SymptomReportDto dto) => new()
        {
            Id = dto.Id,
            Symptom = FromDto(dto.Symptom),
            Medication = dto.Medication is null ? null : FromDto(dto.Medication),
            SubmittedAt = ToUtc(dto.SubmittedAtLocal),
            SubmittedFor = ToUtc(dto.SubmittedForLocal),
            Severity = dto.Severity,
            Notes = dto.Notes,
            IsNew = dto.IsNew
        };

        public SymptomStateChangeDto ToDto(SymptomStateChange model) => new()
        {
            Id = model.Id,
            SymptomId = model.SymptomId,
            ChangedAtLocal = ToLocal(model.ChangedAt),
            SymptomBefore = ToDto(model.SymptomBefore),
            SymptomAfter = ToDto(model.SymptomAfter),
            IsNew = model.IsNew
        };

        public SymptomStateChange FromDto(SymptomStateChangeDto dto) => new()
        {
            Id = dto.Id,
            SymptomId = dto.SymptomId,
            ChangedAt = ToUtc(dto.ChangedAtLocal),
            SymptomBefore = FromDto(dto.SymptomBefore),
            SymptomAfter = FromDto(dto.SymptomAfter),
            IsNew = dto.IsNew
        };

        public MedicineDto ToDto(Medicine model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            Dose = model.Dose,
            PrescribedBy = model.PrescribedBy,
            Notes = model.Notes,
            PrescribedAtLocal = ToLocal(model.PrescribedAt),
            MedicineScheduleId = model.MedicineScheduleId,
            MedicineSchedule = model.MedicineSchedule is null ? new MedicineScheduleDto() : ToDto(model.MedicineSchedule),
            StateChanges = MapList(model.StateChanges, ToDto),
            MedicineOccurances = MapList(model.MedicineOccurances, ToDto),
            Active = model.Active,
            IsNew = model.IsNew,
            MedicineReports = MapList(model.MedicineReports, ToDto)
        };

        public Medicine FromDto(MedicineDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Dose = dto.Dose,
            PrescribedBy = dto.PrescribedBy,
            Notes = dto.Notes,
            PrescribedAt = ToUtc(dto.PrescribedAtLocal),
            MedicineScheduleId = dto.MedicineScheduleId,
            MedicineSchedule = dto.MedicineSchedule is null ? new MedicineSchedule() : FromDto(dto.MedicineSchedule),
            StateChanges = MapList(dto.StateChanges, FromDto),
            MedicineOccurances = MapList(dto.MedicineOccurances, FromDto),
            Active = dto.Active,
            IsNew = dto.IsNew,
            MedicineReports = MapList(dto.MedicineReports, FromDto)
        };

        public MedicineScheduleDto ToDto(MedicineSchedule model) => new()
        {
            Id = model.Id,
            Type = model.Type,
            IntervalType = model.IntervalType,
            IntervalValue = model.IntervalValue,
            AmountPerDay = model.AmountPerDay,
            StartedAtLocal = ToLocal(model.StartedAt),
            IsNew = model.IsNew
        };

        public MedicineSchedule FromDto(MedicineScheduleDto dto) => new()
        {
            Id = dto.Id,
            Type = dto.Type,
            IntervalType = dto.IntervalType,
            IntervalValue = dto.IntervalValue,
            AmountPerDay = dto.AmountPerDay,
            StartedAt = ToUtc(dto.StartedAtLocal),
            IsNew = dto.IsNew
        };

        public MedicineReportDto ToDto(MedicineReport model) => new()
        {
            Id = model.Id,
            MedicineId = model.MedicineId,
            Medicine = ToDto(model.Medicine),
            SubmittedAtLocal = ToLocal(model.SubmittedAt),
            MedicineTakenAtLocal = ToLocal(model.MedicineTakenAt),
            DueAt = ToDto(model.DueAt),
            MedicineTaken = model.MedicineTaken,
            Dose = model.Dose,
            Notes = model.Notes,
            IsNew = model.IsNew
        };

        public MedicineReport FromDto(MedicineReportDto dto) => new()
        {
            Id = dto.Id,
            MedicineId = dto.MedicineId,
            Medicine = FromDto(dto.Medicine),
            SubmittedAt = ToUtc(dto.SubmittedAtLocal),
            MedicineTakenAt = ToUtc(dto.MedicineTakenAtLocal),
            DueAt = dto.DueAt is null ? new MedicineDueAtOccurance() : FromDto(dto.DueAt),
            MedicineTaken = dto.MedicineTaken,
            Dose = dto.Dose,
            Notes = dto.Notes,
            IsNew = dto.IsNew
        };

        public MedicineDueAtOccuranceDto ToDto(MedicineDueAtOccurance model) => new()
        {
            Id = model.Id,
            Medicine = ToDto(model.Medicine),
            Status = model.Status,
            DueAtLocal = ToLocal(model.DueAt),
            CreatedAtLocal = ToLocal(model.CreatedAt)
        };

        public MedicineDueAtOccurance FromDto(MedicineDueAtOccuranceDto dto) => new()
        {
            Id = dto.Id,
            Medicine = FromDto(dto.Medicine),
            Status = dto.Status,
            DueAt = ToUtc(dto.DueAtLocal),
            CreatedAt = ToUtc(dto.CreatedAtLocal)
        };

        public MedicineStateChangeDto ToDto(MedicineStateChange model) => new()
        {
            Id = model.Id,
            MedicineId = model.MedicineId,
            ChangedAtLocal = ToLocal(model.ChangedAt),
            MedicineBefore = ToDto(model.MedicineBefore),
            MedicineAfter = ToDto(model.MedicineAfter),
            IsNew = model.IsNew
        };

        public MedicineStateChange FromDto(MedicineStateChangeDto dto) => new()
        {
            Id = dto.Id,
            MedicineId = dto.MedicineId,
            ChangedAt = ToUtc(dto.ChangedAtLocal),
            MedicineBefore = FromDto(dto.MedicineBefore),
            MedicineAfter = FromDto(dto.MedicineAfter),
            IsNew = dto.IsNew
        };

        public FoodItemDto ToDto(FoodItem model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            CreatedAtLocal = ToLocal(model.CreatedAt),
            FoodReports = MapList(model.FoodReports, ToDto),
            IsNew = model.IsNew
        };

        public FoodItem FromDto(FoodItemDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = ToUtc(dto.CreatedAtLocal),
            FoodReports = MapList(dto.FoodReports, FromDto),
            IsNew = dto.IsNew
        };

        public FoodItemReportDto ToDto(FoodItemReport model) => new()
        {
            Id = model.Id,
            FoodItem = ToDto(model.FoodItem),
            CreatedAtLocal = ToLocal(model.CreatedAt),
            AteFoodAtLocal = ToLocal(model.AteFoodAt),
            Notes = model.Notes,
            IsNew = model.IsNew
        };

        public FoodItemReport FromDto(FoodItemReportDto dto) => new()
        {
            Id = dto.Id,
            FoodItem = FromDto(dto.FoodItem),
            CreatedAt = ToUtc(dto.CreatedAtLocal),
            AteFoodAt = ToUtc(dto.AteFoodAtLocal),
            Notes = dto.Notes,
            IsNew = dto.IsNew
        };

        public MealDto ToDto(Meal model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            FoodItems = MapList(model.FoodItems, ToDto),
            Notes = model.Notes,
            CreatedAtLocal = ToLocal(model.CreatedAt),
            MealReports = MapList(model.MealReports, ToDto),
            IsNew = model.IsNew
        };

        public Meal FromDto(MealDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            FoodItems = MapList(dto.FoodItems, FromDto),
            Notes = dto.Notes,
            CreatedAt = ToUtc(dto.CreatedAtLocal),
            MealReports = MapList(dto.MealReports, FromDto),
            IsNew = dto.IsNew
        };

        public MealReportDto ToDto(MealReport model) => new()
        {
            Id = model.Id,
            Meal = ToDto(model.Meal),
            CreatedAtLocal = ToLocal(model.CreatedAt),
            AteMealAtLocal = ToLocal(model.AteMealAt),
            Notes = model.Notes,
            IsNew = model.IsNew
        };

        public MealReport FromDto(MealReportDto dto) => new()
        {
            Id = dto.Id,
            Meal = FromDto(dto.Meal),
            CreatedAt = ToUtc(dto.CreatedAtLocal),
            AteMealAt = ToUtc(dto.AteMealAtLocal),
            Notes = dto.Notes,
            IsNew = dto.IsNew
        };

        public ScheduledNotificationDto ToDto(ScheduledNotification model) => new()
        {
            Id = model.Id,
            Type = model.Type,
            StartAtLocal = ToLocal(model.StartAt),
            CreatedAtLocal = ToLocal(model.CreatedAt),
            LastSentAtLocal = ToLocal(model.LastSentAt),
            IntervalType = model.IntervalType,
            IntervalValue = model.IntervalValue,
            IsNew = model.IsNew,
            Active = model.Active
        };

        public ScheduledNotification FromDto(ScheduledNotificationDto dto) => new(dto.Type)
        {
            Id = dto.Id,
            StartAt = ToUtc(dto.StartAtLocal),
            CreatedAt = ToUtc(dto.CreatedAtLocal),
            LastSentAt = ToUtc(dto.LastSentAtLocal),
            IntervalType = dto.IntervalType,
            IntervalValue = dto.IntervalValue,
            IsNew = dto.IsNew,
            Active = dto.Active
        };
    }
}