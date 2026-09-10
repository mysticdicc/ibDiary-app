using Android.Content;
using AndroidX.Work;
using ibDiary_app.Services.System;
using ibDiary_data.Data;
using ibDiary_data.Models.Food;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Stats;
using ibDiary_data.Models.Symptoms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ibDiary_app.Services.Stats
{
    public class StatsGenerationService(
        IDbContextFactory<AppDbContext> dbFactory,
        ClientNotificationService notifier,
        ComponentUpdateService updater,
        DateLocalisationService dateService)
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory = dbFactory;
        private readonly ClientNotificationService _notifier = notifier;
        private readonly ComponentUpdateService _updater = updater;
        private SemaphoreSlim _semaphore = new(1, 1);
        private TimeSpan _debounce = TimeSpan.FromSeconds(10);
        private CancellationTokenSource? cts;
        private readonly object _debounceLock = new();
        private readonly DateLocalisationService _dateService = dateService;

        public Task RequestStatsUpdateAsync()
        {
            CancellationToken token;
            TimeSpan delay = _debounce;
            lock (_debounceLock)
            {
                cts?.Cancel();
                cts?.Dispose();
                cts = new CancellationTokenSource();
                token = cts.Token;
            }

            _ = Task.Run(async () =>
            {
                bool locked = false;

                try
                {
                    await Task.Delay(delay, token);
                    await _semaphore.WaitAsync(token);
                    locked = true;
                    await GenerateStatsSnapshotAsync(DateOnly.FromDateTime(_dateService.GetCurrentLocalTime()));
                }
                catch (OperationCanceledException)
                { }
                catch (Exception ex)
                {
                    _notifier.ShowNotification("Stats Generation Error", ex.Message);
                }

                if (locked) _semaphore.Release();
            });

            return Task.CompletedTask;
        }

        public async Task<StatsSnapshot> GenerateStatsSnapshotAsync(DateOnly monthEnd)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            var snapshot = new StatsSnapshot(monthEnd);
            await GenerateStats(snapshot, monthEnd, context);

            var dbItem = await context.StatsSnapshots
                .AsSplitQuery()
                .Include(x => x.MedicineStats)
                    .ThenInclude(x => x.Medicine)
                .Include(x => x.MedicineStats)
                    .ThenInclude(x => x.MedicineTakenTrend)
                .Include(x => x.SymptomStats)
                    .ThenInclude(x => x.Symptom)
                .Include(x => x.SymptomStats)
                    .ThenInclude(x => x.MonthlySeverityTrend)
                .Include(x => x.FoodStats)
                    .ThenInclude(x => x.Food)
                .Include(x => x.FoodStats)
                    .ThenInclude(x => x.FoodEatenByHour)
                .Include(x => x.MealStats)
                    .ThenInclude(x => x.Meal)
                .Include(x => x.MealStats)
                    .ThenInclude(x => x.MealEatenByHour)
                .Where(x => x.MonthEnd == monthEnd)
                .FirstOrDefaultAsync();

            if (dbItem != null)
            {
                dbItem.UpdateProperties(snapshot);
            }
            else
            {
                await context.StatsSnapshots.AddAsync(snapshot);
            }

            await context.SaveChangesAsync();

            _updater.NotifiyComponentUpdate(null);
            return dbItem ?? snapshot;
        }

        public async Task GenerateStats(StatsSnapshot snapshot, DateOnly monthEnd, AppDbContext context)
        {
            snapshot.MonthEnd = monthEnd;

            snapshot.MedicineStats = [];
            snapshot.SymptomStats = [];
            snapshot.FoodStats = [];
            snapshot.MealStats = [];

            var medicines = await context.Medicines
                .Include(x => x.MedicineReports)
                .Include(x => x.StateChanges)
                .Include(x => x.MedicineOccurances)
                .ToListAsync();

            snapshot.MedicineCount = medicines.Count;
            snapshot.ActiveMedicineCount = medicines.Count(x => x.Active);
            snapshot.TotalMedicineReports = medicines.Sum(x => x.MedicineReports.Count);
            snapshot.MonthlyMedicalReports = medicines.Sum(x =>
                x.MedicineReports.Count(r =>
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) <= snapshot.MonthEnd));
            snapshot.MonthlyMedicinesTaken = medicines.Sum(x =>
                x.MedicineReports.Count(r =>
                    r.MedicineTaken &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) <= snapshot.MonthEnd));

            foreach (var medicine in medicines)
            {
                snapshot.MedicineStats.Add(GenerateMedicineStatsSnapshot(medicine, snapshot.MonthBefore, snapshot.MonthEnd));
            }

            var symptoms = await context.Symptoms
                .Include(x => x.SymptomReports)
                .Include(x => x.StateChanges)
                .ToListAsync();

            snapshot.SymptomCount = symptoms.Count;
            snapshot.ActiveSymptomCount = symptoms.Count(x => x.Active);
            snapshot.TotalSymptomReports = symptoms.Sum(x => x.SymptomReports.Count);
            snapshot.MonthlySymptomReports = symptoms.Sum(x =>
                x.SymptomReports.Count(r =>
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.SubmittedFor)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.SubmittedFor)) <= snapshot.MonthEnd));

            foreach (var symptom in symptoms)
            {
                snapshot.SymptomStats.Add(GenerateSymptomStatsSnapshot(symptom, snapshot.MonthBefore, snapshot.MonthEnd));
            }

            var foods = await context.FoodItems
                .Include(x => x.FoodReports)
                .ToListAsync();

            snapshot.TotalFoodReports = foods.Sum(x => x.FoodReports.Count);
            snapshot.MonthlyFoodReports = foods.Sum(x =>
                x.FoodReports.Count(r =>
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteFoodAt)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteFoodAt)) <= snapshot.MonthEnd));
            snapshot.UniqueMonthlyFoodItems = foods.Count(x =>
                x.FoodReports.Any(r =>
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteFoodAt)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteFoodAt)) <= snapshot.MonthEnd));

            foreach (var food in foods)
            {
                snapshot.FoodStats.Add(GenerateFoodStatsSnapshot(food, snapshot.MonthBefore, snapshot.MonthEnd));
            }

            var meals = await context.Meals
                .Include(x => x.MealReports)
                .ToListAsync();

            snapshot.TotalMealReports = meals.Sum(x => x.MealReports.Count);
            snapshot.MonthlyMealReports = meals.Sum(x =>
                x.MealReports.Count(r =>
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteMealAt)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteMealAt)) <= snapshot.MonthEnd));
            snapshot.UniqueMonthlyMeals = meals.Count(x =>
                x.MealReports.Any(r =>
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteMealAt)) > snapshot.MonthBefore &&
                    DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteMealAt)) <= snapshot.MonthEnd));

            foreach (var meal in meals)
            {
                snapshot.MealStats.Add(GenerateMealStatsSnapshot(meal, snapshot.MonthBefore, snapshot.MonthEnd));
            }
        }

        private MedicineStatsSnapshot GenerateMedicineStatsSnapshot(Medicine medicine, DateOnly monthBefore, DateOnly monthEnd)
        {
            var stats = new MedicineStatsSnapshot(medicine);

            stats.TotalReportsCount = medicine.MedicineReports.Count;
            stats.MonthlyReportsCount = medicine.MedicineReports.Count(r =>
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) > monthBefore &&
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) <= monthEnd);

            stats.TotalStateChanges = medicine.StateChanges.Count;
            stats.MonthlyStateChanges = medicine.StateChanges.Count(sc =>
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(sc.ChangedAt)) > monthBefore &&
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(sc.ChangedAt)) <= monthEnd);

            stats.MedicineTakenTrend = [];
            for (var date = monthBefore; date <= monthEnd; date = date.AddDays(1))
            {
                var reportsForDate = medicine.MedicineReports
                    .Where(r => DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.MedicineTakenAt)) == date)
                    .ToList();

                var reportCount = reportsForDate.Count;
                var takenCount = reportsForDate.Count(r => r.MedicineTaken);

                var trend = new MedicineTakenTrendPoint(date)
                {
                    ReportCount = reportCount,
                    AverageTaken = reportCount == 0 ? 0 : ((double)takenCount / reportCount) * 100
                };

                stats.MedicineTakenTrend.Add(trend);
            }

            return stats;
        }

        private SymptomStatsSnapshot GenerateSymptomStatsSnapshot(Symptom symptom, DateOnly monthBefore, DateOnly monthEnd)
        {
            var stats = new SymptomStatsSnapshot(symptom);

            stats.TotalReportsCount = symptom.SymptomReports.Count;
            stats.MonthlyReportsCount = symptom.SymptomReports.Count(r =>
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.SubmittedFor)) > monthBefore &&
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.SubmittedFor)) <= monthEnd);

            stats.TotalStateChanges = symptom.StateChanges.Count;
            stats.MonthlyStateChanges = symptom.StateChanges.Count(sc =>
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(sc.ChangedAt)) > monthBefore &&
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(sc.ChangedAt)) <= monthEnd);

            stats.MonthlySeverityTrend = [];
            for (var date = monthBefore; date <= monthEnd; date = date.AddDays(1))
            {
                var reportsForDate = symptom.SymptomReports
                    .Where(r => DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.SubmittedFor)) == date)
                    .ToList();

                var trend = new SymptomSeverityTrendPoint(date)
                {
                    ReportCount = reportsForDate.Count,
                    AverageSeverity = reportsForDate.Count == 0 ? 0 : reportsForDate.Average(r => r.Severity)
                };

                stats.MonthlySeverityTrend.Add(trend);
            }

            return stats;
        }

        private FoodStatsSnapshot GenerateFoodStatsSnapshot(FoodItem food, DateOnly monthBefore, DateOnly monthEnd)
        {
            var stats = new FoodStatsSnapshot(food);

            stats.TotalReportsCount = food.FoodReports.Count;
            stats.MonthlyReportsCount = food.FoodReports.Count(r =>
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteFoodAt)) > monthBefore &&
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteFoodAt)) <= monthEnd);

            stats.FoodEatenByHour = [];
            for (var date = monthBefore; date <= monthEnd; date = date.AddDays(1))
            {
                for (int i = 1; i < 24; i++)
                {
                    var startHour = new TimeOnly(i, 0, 0);
                    var endHour = startHour.AddHours(1);

                    var count = food.FoodReports.Count(r =>
                    {
                        var localAte = _dateService.UtcToLocalTime(r.AteFoodAt);
                        var localDate = DateOnly.FromDateTime(localAte);
                        var localTime = TimeOnly.FromDateTime(localAte);
                        return localDate == date && localTime >= startHour && localTime < endHour;
                    });

                    var point = new FoodEatenTrendPoint(new DateTime(date.Year, date.Month, date.Day, i, 0, 0))
                    {
                        Count = count
                    };

                    stats.FoodEatenByHour.Add(point);
                }
            }

            return stats;
        }

        private MealStatsSnapshot GenerateMealStatsSnapshot(Meal meal, DateOnly monthBefore, DateOnly monthEnd)
        {
            var stats = new MealStatsSnapshot(meal);

            stats.TotalReportsCount = meal.MealReports.Count;
            stats.MonthlyReportsCount = meal.MealReports.Count(r =>
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteMealAt)) > monthBefore &&
                DateOnly.FromDateTime(_dateService.UtcToLocalTime(r.AteMealAt)) <= monthEnd);

            stats.MealEatenByHour = [];
            for (var date = monthBefore; date <= monthEnd; date = date.AddDays(1))
            {
                for (int i = 1; i < 24; i++)
                {
                    var startHour = new TimeOnly(i, 0, 0);
                    var endHour = startHour.AddHours(1);

                    var count = meal.MealReports.Count(r =>
                    {
                        var localAte = _dateService.UtcToLocalTime(r.AteMealAt);
                        var localDate = DateOnly.FromDateTime(localAte);
                        var localTime = TimeOnly.FromDateTime(localAte);
                        return localDate == date && localTime >= startHour && localTime < endHour;
                    });

                    var point = new MealEatenTrendPoint(new DateTime(date.Year, date.Month, date.Day, i, 0, 0))
                    {
                        Count = count
                    };

                    stats.MealEatenByHour.Add(point);
                }
            }

            return stats;
        }
    }
}