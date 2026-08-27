using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Work;
using ibDiary_data.Models.Settings;
using ibDiary_app.Services.Medication;
using ibDiary_app.Services.Settings;
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
            _notificationRepo = null;
        }

        private PendingMedicineReportService? _medReportService;
        private AppSettings? _settings;
        private ScheduledNotificationRepository? _notificationRepo;

        public override Result DoWork()
        {
            return DoWorkAsync().GetAwaiter().GetResult()!;
        }

        private bool ServicesNeedLoading()
        {
            if (_settings == null) return true;
            if (_medReportService == null) return true;
            if (_notificationRepo == null) return true;
            return false;
        }

        private void LoadServices()
        {
            var services = IPlatformApplication.Current?.Services;
            _medReportService = services?.GetService<PendingMedicineReportService>();
            _settings = services?.GetService<AppSettings>();
            _notificationRepo = services?.GetService<ScheduledNotificationRepository>();
        }

        public async Task<Result?> DoWorkAsync()
        {
            try
            {
                if (ServicesNeedLoading()) LoadServices();
                if (ServicesNeedLoading()) return Result.InvokeRetry();
                if (!_settings!.NotificationsEnabled) return Result.InvokeSuccess();
                if (_settings.MedicineReportNotificationsEnabled) await HandleMedicineReportNotificationsAsync();
                if (_settings.ScheduledNotificationsEnabled) await HandleScheduledNotifications();

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
                    SendNotification("IbDiary Medicine Reminder", $"You have {reports.Count} medicines due reports.");
                }
            }
        }

        private async Task HandleScheduledNotifications()
        {
            var notifications = await _notificationRepo!.GetAllAsync();
            var active = notifications.Where(x => x.Active).ToList();
            if (active.Count == 0) return;

            foreach (var notification in active)
            {
                var dueAt = CalculateNextDueTime(notification);

                if (dueAt <= DateTime.UtcNow)
                {
                    SendNotification(
                        $"IbDiary - {notification.Type}",
                        $"Time for your {notification.Type} reminder."
                    );

                    notification.LastSentAt = DateTime.UtcNow;
                    await _notificationRepo.UpdateAsync(notification);
                }
            }
        }

        private DateTime CalculateNextDueTime(ScheduledNotification notification)
        {
            var startDate = notification.LastSentAt == DateTime.MinValue
                ? notification.StartAt
                : notification.LastSentAt;

            return notification.IntervalType switch
            {
                ScheduleIntervalType.Minutes => startDate.AddMinutes(notification.IntervalValue),
                ScheduleIntervalType.Hours => startDate.AddHours(notification.IntervalValue),
                ScheduleIntervalType.Days => startDate.AddDays(notification.IntervalValue),
                ScheduleIntervalType.Months => startDate.AddMonths(notification.IntervalValue),
                _ => DateTime.MinValue
            };
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
