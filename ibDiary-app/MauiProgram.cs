using AndroidX.Work;
using ibDiary_data.Data;
using ibDiary_data.Models.Medication;
using ibDiary_data.Models.Settings;
using ibDiary_data.Models.Symptoms;
using ibDiary_app.Services;
using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Food;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.Settings;
using ibDiary_app.Services.Symptoms;
using ibDiary_app.Services.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ibDiary_app.Services.Stats;
using ApexCharts;

namespace ibDiary_app
{
    public static class MauiProgram
    {
        public static async Task RequestNotificationPermission()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android &&
                DeviceInfo.Version.Major >= 13)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }
            }
        }

        public static void SetupMedicineReminders()
        {
            var context = Android.App.Application.Context;

            WorkManager.GetInstance(context).CancelUniqueWork("medicine_reminder_work");

            var workRequest = new PeriodicWorkRequest.Builder(
                typeof(AndroidNotificationService),
                TimeSpan.FromMinutes(15)
            ).Build();

            WorkManager.GetInstance(context)
                .EnqueueUniquePeriodicWork(
                    "medicine_reminder_work",
                    ExistingPeriodicWorkPolicy.CancelAndReenqueue!,
                    workRequest
                );
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "ibdiary_db.db");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}")
            );

            var settings = new AppSettings();
            settings.Load();
            builder.Services.AddSingleton(settings);

            builder.Services.AddApexChartsMaui();

            builder.Services.AddSingleton<ClientNotificationService>();
            builder.Services.AddSingleton<ConfirmationService>();
            builder.Services.AddSingleton<ComponentUpdateService>();

            builder.Services.AddScoped<CalendarRepositoryService>();
            builder.Services.AddScoped<CalendarClientService>();
            builder.Services.AddScoped<CalendarDayGenerationService>();

            builder.Services.AddScoped<StatsSnapshotRepository>();
            builder.Services.AddScoped<StatsSnapshotClientService>();
            builder.Services.AddScoped<StatsGenerationService>();

            builder.Services.AddScoped<MedicineOccuranceRepository>();
            builder.Services.AddScoped<MedicineOccuranceClientService>();

            builder.Services.AddScoped<MedicineStateChangeRepository>();
            builder.Services.AddScoped<MedicineStateChangeClientService>();

            builder.Services.AddScoped<MedicineRepository>();
            builder.Services.AddScoped<MedicineClientService>();

            builder.Services.AddScoped<MedicineReportRepository>();
            builder.Services.AddScoped<MedicineReportClientService>();

            builder.Services.AddScoped<SymptomReportRepository>();
            builder.Services.AddScoped<SymptomReportClientService>();

            builder.Services.AddScoped<SymptomStateChangeRepository>();
            builder.Services.AddScoped<SymptomStateChangeClientService>();

            builder.Services.AddScoped<SymptomRepository>();
            builder.Services.AddScoped<SymptomClientService>();

            builder.Services.AddScoped<PendingMedicineReportService>();

            builder.Services.AddScoped<FoodItemRepository>();
            builder.Services.AddScoped<FoodItemClientService>();

            builder.Services.AddScoped<FoodItemReportRepository>();
            builder.Services.AddScoped<FoodItemReportClientService>();

            builder.Services.AddScoped<MealRepository>();
            builder.Services.AddScoped<MealClientService>();

            builder.Services.AddScoped<MealReportRepository>();
            builder.Services.AddScoped<MealReportClientService>();

            builder.Services.AddScoped<ScheduledNotificationRepository>();
            builder.Services.AddScoped<ScheduledNotificationClientService>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Roboto-Regular", "Roboto");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }
    }
}
