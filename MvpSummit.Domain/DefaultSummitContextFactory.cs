using Microsoft.EntityFrameworkCore;

namespace MvpSummit.Domain
{
    public class DefaultSummitContextFactory : ISummitTaskContextFactory
    {
        private readonly IDbContextFactory<SummitTaskContext> factory;

        public DefaultSummitContextFactory(IDbContextFactory<SummitTaskContext> factory) =>
            this.factory = factory;

        public async Task<SummitTaskContext> CreateSummitContextAsync() =>
            await factory.CreateDbContextAsync();
    }
}
