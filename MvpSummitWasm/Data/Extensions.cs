using Microsoft.EntityFrameworkCore;
using MvpSummit.Domain;

namespace MvpSummitWasm.Data
{
    public static class Extensions
    {
        public static IServiceCollection AddSynchronizingDataFactory(
            this IServiceCollection service) =>
            service.AddSingleton<ISummitTaskContextFactory, SynchronizedSummitDbContextFactory>();            
    }
}
