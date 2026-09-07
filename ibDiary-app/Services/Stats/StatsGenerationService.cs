using Android.Content;
using AndroidX.Work;
using ibDiary_app.Services.System;
using ibDiary_data.Data;
using ibDiary_data.Models.Stats;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Stats
{
    public class StatsGenerationService(
        IDbContextFactory<AppDbContext> dbFactory, 
        ClientNotificationService notifier,
        ComponentUpdateService updater)
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory = dbFactory;
        private readonly ClientNotificationService _notifier = notifier;
        private readonly ComponentUpdateService _updater = updater;
        private SemaphoreSlim _semaphore = new(1, 1);
        private TimeSpan _debounce = TimeSpan.FromSeconds(10);
        private CancellationTokenSource? cts;
        private readonly object _debounceLock = new();

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
                    await GenerateStatsSnapshotAsync(DateOnly.FromDateTime(DateTime.UtcNow));
                }
                catch(OperationCanceledException)
                { }
                catch(Exception ex)
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
            await snapshot.GenerateStats(context, monthEnd);

            var dbItem = await context.StatsSnapshots.Where(x => x.MonthEnd == monthEnd).FirstOrDefaultAsync();

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
    }
}
