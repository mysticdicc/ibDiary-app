using ibDiary_app.Data;
using ibDiary_app.Models.Medication;
using ibDiary_app.Models.Symptoms;
using ibDiary_app.Services;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.Symptoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ibDiary_app
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "ibdiary_db.db");
#if DEBUG
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
#endif
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}")
            );

            builder.Services.AddSingleton<NotificationService>();

            builder.Services.AddSingleton<MedicineRepository>();
            builder.Services.AddSingleton<MedicineDatabaseService>();

            builder.Services.AddSingleton<MedicineReportRepository>();
            builder.Services.AddSingleton<MedicineReportDatabaseService>();

            builder.Services.AddSingleton<SymptomReportRepository>();
            builder.Services.AddSingleton<SymptomReportDatabaseService>();

            builder.Services.AddSingleton<SymptomStateChangeRepository>();
            builder.Services.AddSingleton<SymptomRepository>();
            builder.Services.AddSingleton<SymptomDatabaseService>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
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
