using ibDiary_app.Services;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.Symptoms;
using Microsoft.Extensions.Logging;
using SQLite;

namespace ibDiary_app
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "ibdiary_db.db");

            builder.Services.AddSingleton(sp =>
            {
                var conn = new SQLiteAsyncConnection(dbPath);
                return conn;
            });

            builder.Services.AddSingleton<NotificationService>();

            builder.Services.AddSingleton<MedicineRepository>();
            builder.Services.AddSingleton<MedicineDatabaseService>();

            builder.Services.AddSingleton<MedicineReportRepository>();
            builder.Services.AddSingleton<MedicineReportDatabaseService>();

            builder.Services.AddSingleton<SymptomReportRepository>();
            builder.Services.AddSingleton<SymptomReportDatabaseService>();

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
