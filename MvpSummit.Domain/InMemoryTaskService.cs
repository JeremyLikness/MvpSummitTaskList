namespace MvpSummit.Domain
{
    public class InMemoryTaskService : ISummitTaskService
    {
        private readonly List<SummitTask> summitTasks = new();

        public Task<SummitTask> AddTaskAsync(string task)
        {
            summitTasks.Add(new SummitTask
            {
                Id = summitTasks.Any() ? 
                    summitTasks.Max(t => t.Id) + 1 :
                    1,
                Name = task
            });
            return Task.FromResult(summitTasks[^1]);
        }

        public Task<SummitTask[]> GetTasksAsync() => Task.FromResult(summitTasks.ToArray());

        public Task MarkDoneAsync(int taskId)
        {
            summitTasks.Single(t => t.Id == taskId).IsComplete = true;
            return Task.CompletedTask;
        }
    }
}
