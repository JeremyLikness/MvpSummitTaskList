using Microsoft.EntityFrameworkCore;

namespace MvpSummit.Domain
{
    public class DbSummitTaskService : ISummitTaskService
    {
        private readonly ISummitTaskContextFactory dbContextFactory;
        public DbSummitTaskService(ISummitTaskContextFactory dbContextFactory)
            => this.dbContextFactory = dbContextFactory;

        public async Task<SummitTask> AddTaskAsync(string task)
        {
            using var ctx = await dbContextFactory.CreateSummitContextAsync();
            var newTask = new SummitTask { Name = task };
            ctx.SummitTasks.Add(newTask);
            await ctx.SaveChangesAsync();
            return newTask;
        }

        public async Task<SummitTask[]> GetTasksAsync() 
        {
            using var ctx = await dbContextFactory.CreateSummitContextAsync();
            return ctx.SummitTasks.ToArray();
        }

        public async Task MarkDoneAsync(int taskId)
        {
            using var ctx = await dbContextFactory.CreateSummitContextAsync();  
            var task = await ctx.FindAsync<SummitTask>(taskId);
            if (task != null)
            {
                task.IsComplete = true;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
