using Android.Content;
using AndroidX.Work;
using ibDiary_app.Services.System;
using ibDiary_data.Data;
using ibDiary_data.Models.Stats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Stats
{
    public class StatsGenerationService(
        AppDbContext dbContext, 
        StatsSnapshotRepository repo, 
        ClientNotificationService notifier,
        ComponentUpdateService updater)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly StatsSnapshotRepository _repo = repo;
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
            var snapshot = new StatsSnapshot(monthEnd);
            await snapshot.GenerateStats(_dbContext, monthEnd);

            var dbItem = await _repo.GetByDateAsync(monthEnd);
            if (dbItem != null)
            {
                dbItem.UpdateProperties(snapshot);
                await _repo.UpdateAsync(dbItem);
            }
            else
            {
                await _repo.AddAsync(snapshot);
            }

            _updater.NotifiyComponentUpdate(null);
            return dbItem ?? snapshot;
        }
    }
}
