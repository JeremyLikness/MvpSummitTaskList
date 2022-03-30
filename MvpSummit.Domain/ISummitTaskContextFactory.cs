namespace MvpSummit.Domain
{
    public interface ISummitTaskContextFactory
    {
        Task<SummitTaskContext> CreateSummitContextAsync();
    }
}
