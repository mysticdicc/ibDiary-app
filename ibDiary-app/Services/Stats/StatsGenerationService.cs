using Android.Content;
using AndroidX.Work;
using ibDiary_data.Data;
using ibDiary_data.Models.Stats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_app.Services.Stats
{
    public class StatsGenerationService(Context context, WorkerParameters parameters) : Worker(context, parameters)
    {
        private AppDbContext? _dbContext;
        private StatsSnapshotRepository? _repo;

        public override Result DoWork()
        {
            return DoWorkAsync().GetAwaiter().GetResult()!;
        }

        public async Task<Result?> DoWorkAsync()
        {
            try
            {
                if (ServicesNeedLoading())
                {
                    LoadServices();
                    if (ServicesNeedLoading()) return Result.InvokeRetry();
                }

                var date = DateOnly.FromDateTime(DateTime.UtcNow);
                await GenerateStatsSnapshotAsync(date);
                return Result.InvokeSuccess();
            }
            catch
            {
                return Result.InvokeRetry();
            }
        }

        private bool ServicesNeedLoading()
        {
            if (_dbContext == null) return true;
            if (_repo == null) return true;
            return false;
        }

        private void LoadServices()
        {
            var services = IPlatformApplication.Current?.Services;
            _dbContext = services?.GetService<AppDbContext>();
            _repo = services?.GetService<StatsSnapshotRepository>();
        }

        public async Task<StatsSnapshot> GenerateStatsSnapshotAsync(DateOnly monthEnd)
        {
            var snapshot = new StatsSnapshot(monthEnd);
            await snapshot.GenerateStats(_dbContext!, monthEnd);

            var dbItem = await _repo?.GetByDateAsync(monthEnd) ?? null;
            if (dbItem != null)
            {
                dbItem.UpdateProperties(snapshot);
                await _repo.UpdateAsync(dbItem);
            }
            else
            {
                await _repo.AddAsync(snapshot);
            }

            return dbItem ?? snapshot;
        }
    }
}
