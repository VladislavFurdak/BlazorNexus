using BlazorNexsus.Navigation.BrowserSpecific;
using BlazorNexsus.Navigation.Repositories;
using Microsoft.Extensions.DependencyInjection;

internal static class DI
{
    public static IServiceCollection RegisterServices<T>(IServiceCollection serviceCollection) where T : struct, Enum
    {
        serviceCollection.AddSingleton<IBackPageRepository<T>, BackPageRepository<T>>();
        serviceCollection.AddSingleton<ISessionStorageRepository, SessionStorageRepository>();
        return serviceCollection;
    }
}
