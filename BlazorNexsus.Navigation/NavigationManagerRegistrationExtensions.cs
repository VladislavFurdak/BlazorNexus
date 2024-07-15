using System.Reflection;
using BlazorNexsus.Navigation.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazorNexsus.Navigation
{
    public static class DependencyInjection
    {
        /*
        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services,
            Assembly[] assembliesWithViews,
            bool forcePagePostfixCheck = true,
            bool forceCheckUnusedKeys = true) where T : struct, Enum
        {
            foreach (var assmebly in assembliesWithViews)
            {
                if (forcePagePostfixCheck)
                {
                    CheckerUtils.CheckPagesPostfixes(assmebly);
                }
            
                if (forceCheckUnusedKeys)
                {
                    CheckerUtils.CheckUnusedKeys(assmebly);
                }
            }
            
            var jsRuntime = services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
            
            DI.RegisterServices<T>();
            services.AddScoped<INavigationManager<T>, NavigationManagerExt<T>>(provider =>
                new NavigationManagerExt<T>(assembliesWithViews,
                    (NavigationManager) provider.GetService(typeof(NavigationManager)),
                    jsRuntime)
                );
        }*/

        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services, 
            bool forcePagePostfixCheck = true,
            bool forceCheckUnusedKeys = true) where T : struct, Enum
        {
            if (forcePagePostfixCheck)
            {
                CheckerUtils.CheckPagesPostfixes(typeof(T).Assembly);
            }
            
            if (forceCheckUnusedKeys)
            {
                CheckerUtils.CheckUnusedKeys(typeof(T).Assembly);
            }
            
            DI.RegisterServices<T>(services);
            
            services.AddScoped<INavigationManager<T>, NavigationManagerExt<T>>();
            
            /*provider =>
                new NavigationManagerExt<T>(
                    new[] {typeof(T).Assembly},
                    (NavigationManager) provider.GetService(typeof(NavigationManager)),
                    jsRuntime)*/
        }
/*
        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services,
            Assembly assemblyWithViews, bool forcePagePostfixCheck = true) where T : struct, Enum
        {
            AddEasyBlazorNavigationManager<T>(services, new[] {assemblyWithViews});
        }

        public static void AddEasyBlazorNavigationManager<T>(
            this IServiceCollection services,
            Type anyAssemblyType, bool forcePagePostfixCheck = true) where T : struct, Enum
        {
            AddEasyBlazorNavigationManager<T>(services, new[] {Assembly.GetAssembly(anyAssemblyType)});
        }*/
    }
}