using EasyBlazor.Navigation.BrowserSpecific;
using EasyBlazor.Navigation.Repositories;
using Microsoft.Extensions.DependencyInjection;

public static class DI
{
    private static IServiceProvider _serviceProvider;
    
    public static T? Get<T>()
    {
        return _serviceProvider.GetService<T>();
    }
    
    public static void RegisterServices<T>() where T : Enum
    {
        var services = new ServiceCollection();
        services.AddSingleton<IBackpageRepository<T>, BackpageRepository<T>>();
        services.AddSingleton<ICurrentPageRepository<T>, CurrentPageRepository<T>>();

        services.AddSingleton<ILocalStorageRepository, LocalStorageRepository>();
        services.AddSingleton<ISessionStorageRepository, SessionStorageRepository>();

        _serviceProvider = services.BuildServiceProvider();
    }
}