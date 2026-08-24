using AndroidX.Work;
using ibDiary_app.Data;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Settings;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services;
using ibDiary_app.Services.Calendar;
using ibDiary_app.Services.Food;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.Symptoms;
using ibDiary_app.Services.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

            builder.Services.AddSingleton<ClientNotificationService>();
            builder.Services.AddSingleton<ConfirmationService>();
            builder.Services.AddSingleton<ComponentUpdateService>();

            builder.Services.AddSingleton<CalendarRepositoryService>();
            builder.Services.AddSingleton<CalendarClientService>();
            builder.Services.AddSingleton<CalendarDayGenerationService>();

            builder.Services.AddSingleton<MedicineStateChangeRepository>();
            builder.Services.AddSingleton<MedicineStateChangeClientService>();

            builder.Services.AddSingleton<MedicineRepository>();
            builder.Services.AddSingleton<MedicineClientService>();

            builder.Services.AddSingleton<MedicineReportRepository>();
            builder.Services.AddSingleton<MedicineReportClientService>();

            builder.Services.AddSingleton<SymptomReportRepository>();
            builder.Services.AddSingleton<SymptomReportClientService>();

            builder.Services.AddSingleton<SymptomStateChangeRepository>();
            builder.Services.AddSingleton<SymptomStateChangeClientService>();

            builder.Services.AddSingleton<SymptomRepository>();
            builder.Services.AddSingleton<SymptomClientService>();

            builder.Services.AddSingleton<PendingMedicineReportService>();

            builder.Services.AddSingleton<FoodItemRepository>();
            builder.Services.AddSingleton<FoodItemClientService>();

            builder.Services.AddSingleton<FoodItemReportRepository>();
            builder.Services.AddSingleton<FoodItemReportClientService>();

            builder.Services.AddSingleton<MealRepository>();
            builder.Services.AddSingleton<MealClientService>();

            builder.Services.AddSingleton<MealReportRepository>();
            builder.Services.AddSingleton<MealReportClientService>();

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

            return builder.Build();
        }
    }
}
