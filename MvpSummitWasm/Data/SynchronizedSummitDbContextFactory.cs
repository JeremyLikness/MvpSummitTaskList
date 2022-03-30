using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MvpSummit.Domain;

namespace MvpSummitWasm.Data
{
    public class SynchronizedSummitDbContextFactory : ISummitTaskContextFactory
    {
        private readonly IDbContextFactory<SummitTaskContext> dbContextFactory;
        private readonly IJSRuntime Js;
        private Task<int>? lastTask = null;
        private int lastStatus = -2;
        private bool init = false;

        public const string dbFilename = "todos.sqlite3";

        public SynchronizedSummitDbContextFactory(
            IJSRuntime js,
            IDbContextFactory<SummitTaskContext> dbContextFactory)
        {
            Js = js;
            this.dbContextFactory = dbContextFactory;
            lastTask = SynchronizeAsync();
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<SummitTaskContext> CreateSummitContextAsync()
        {
            await CheckForPendingTasksAsync();
            var ctx = await dbContextFactory.CreateDbContextAsync();

            if (!init)
            {
                Console.WriteLine($"Last status: {lastStatus}");
                await ctx.Database.EnsureCreatedAsync();
                init = true;
            }

            ctx.SavedChanges += Ctx_SavedChanges;

            return ctx;
        }

        private async Task CheckForPendingTasksAsync()
        {
            if (lastTask != null)
            {
                lastStatus = await lastTask;
                lastTask.Dispose();
                lastTask = null;
                if (lastStatus == 0)
                {
                    Restore();
                }
            }
        }

        private void Ctx_SavedChanges(object? sender, SavedChangesEventArgs e) =>
            lastTask = SynchronizeAsync();

        private async Task<int> SynchronizeAsync()
        {
            if (init)
            {
                Backup();
            }

            var result = await Js.InvokeAsync<int>("db.synchronizeDbWithCache", dbFilename);
            var resultText = result == -1 ? "Failure" : (result == 0 ? "Restored" : "Cached");
            Console.WriteLine($"Synchronization status: {resultText}");
            return result;
        }

        private static void Backup()
        {
            Console.WriteLine("Begin backup.");
/*
            using var src = new SqliteConnection($"Data Source={dbFilename}");
            using var bak = new SqliteConnection($"Data Source={dbFilename}_bak");

            src.Open();
            bak.Open();

            src.BackupDatabase(bak);

            bak.Close();
            src.Close();
*/
            Console.WriteLine("End backup.");
        }

        private static void Restore()
        {
            Console.WriteLine("Begin restore.");
/*
            using var src = new SqliteConnection($"Data Source={dbFilename}_bak");
            using var bak = new SqliteConnection($"Data Source={dbFilename}");

            src.Open();
            bak.Open();

            src.BackupDatabase(bak);

            bak.Close();
            src.Close();
*/
            Console.WriteLine("End restore.");
        }
    }
}
