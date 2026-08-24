using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Work;
using ibDiary_app.Models.Settings;
using ibDiary_app.Services.Medication;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.System
{
    public class AndroidNotificationService : Worker
    {
        public AndroidNotificationService(Context context, WorkerParameters parameters)
            : base(context, parameters) 
        {
            _medReportService = null;
            _settings = null;
        }

        private PendingMedicineReportService? _medReportService;
        private AppSettings? _settings;

        public override Result DoWork()
        {
            return DoWorkAsync().GetAwaiter().GetResult()!;
        }

        private bool ServicesNeedLoading()
        {
            if (_settings == null) return true;
            if (_medReportService == null) return true;
            return false;
        }

        private void LoadServices()
        {
            var services = IPlatformApplication.Current?.Services;
            _medReportService = services?.GetService<PendingMedicineReportService>();
            _settings = services?.GetService<AppSettings>();
        }

        public async Task<Result?> DoWorkAsync()
        {
            try
            {
                if (ServicesNeedLoading()) LoadServices();
                if (ServicesNeedLoading()) return Result.InvokeRetry();
                if (!_settings!.NotificationsEnabled) return Result.InvokeSuccess();
                if (_settings.MedicineReportNotificationsEnabled) await HandleMedicineReportNotificationsAsync();

                return Result.InvokeSuccess();
            }
            catch
            {
                return Result.InvokeRetry();
            }
        }

        private async Task HandleMedicineReportNotificationsAsync()
        {
            var reports = await _medReportService!.GetPendingReportsAsync();
            if (reports.Count > 0)
            {
                if (reports.Count == 1)
                {
                    SendNotification("IbDiary Medicine Reminder", $"You are due to take {reports.First().Medicine.Name}.");
                }
                else if (reports.Count > 1)
                {
                    SendNotification("IbDiary Medicine Reminder", $"You have {reports.Count} medicines due.");
                }
            }
        }

        private static readonly string CHANNEL_ID = "ibdiary_notifications";

        public static void SendNotification(string title, string message)
        {
            var context = Android.App.Application.Context;
            if (context == null) return;

            CreateNotificationChannel(context);

            var intent = new Intent(context, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var builder = new NotificationCompat.Builder(context, CHANNEL_ID);
            if (builder == null) return;
            builder.SetContentTitle(title);
            builder.SetContentText(message);
            builder.SetSmallIcon(Android.Resource.Drawable.IcDialogInfo);
            builder.SetAutoCancel(true);
            builder.SetPriority(NotificationCompat.PriorityDefault);
            builder.SetContentIntent(pendingIntent);

            var notification = builder.Build();
            if (notification == null) return;

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager?.Notify(1, notification);
        }

        private static void CreateNotificationChannel(Context context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(CHANNEL_ID, "ibDiary Notifications",NotificationImportance.Default);

                var notificationManager = context.GetSystemService(Context.NotificationService)
                    as NotificationManager;
                notificationManager?.CreateNotificationChannel(channel);
            }
        }
    }
}
