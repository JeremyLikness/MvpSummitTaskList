namespace MvpSummit.Domain
{
    public interface ISummitTaskService
    {
        Task<SummitTask[]> GetTasksAsync();
        Task MarkDoneAsync(int taskId);
        Task<SummitTask> AddTaskAsync(string task);
    }
}
