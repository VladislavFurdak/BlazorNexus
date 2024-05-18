using System.Reflection;
using BlazorNexsus.Navigation.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorNexsus.Navigation
{
    public static class DependencyInjection
    {
        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services,
            Assembly?[] assembliesWithViews) where T : struct, Enum
        {
            DI.RegisterServices<T>();
            services.AddScoped<INavigationManager<T>, NavigationManagerExt<T>>(provider =>
                new NavigationManagerExt<T>(assembliesWithViews,
                    (NavigationManager) provider.GetService(typeof(NavigationManager))));
        }

        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services) where T : struct, Enum
        {
            DI.RegisterServices<T>();
            services.AddScoped<INavigationManager<T>, NavigationManagerExt<T>>(provider =>
                new NavigationManagerExt<T>(new[] {typeof(T).Assembly},
                    (NavigationManager) provider.GetService(typeof(NavigationManager))));
        }

        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services,
            Assembly assemblyWithViews) where T : struct, Enum
        {
            AddEasyBlazorNavigationManager<T>(services, new[] {assemblyWithViews});
        }

        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services,
            Type anyAssemblyType) where T : struct, Enum
        {
            AddEasyBlazorNavigationManager<T>(services, new[] {Assembly.GetAssembly(anyAssemblyType)});
        }
    }
}