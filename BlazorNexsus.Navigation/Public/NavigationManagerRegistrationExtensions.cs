using System.Reflection;
using BlazorNexsus.Navigation.Abstractions;
using BlazorNexsus.Navigation.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorNexsus.Navigation
{
    public static class DependencyInjection
    {
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
                    CheckerUtils.CheckUnusedKeys<T>(assmebly);
                }
            }
            
            DI.RegisterServices<T>(services);
            services.AddScoped<AssembliesDTO>(_ => new AssembliesDTO { AssembliesWithViews =  assembliesWithViews});
            services.AddScoped<INavigationManager<T>, NavigationManagerExt<T>>();
        }

        public static void AddBlazorNexusNavigation<T>(
            this IServiceCollection services, 
            bool pagePostfixCheck = true,
            bool checkUnusedKeys = true) where T : struct, Enum
        {
            var assmebly = Assembly.GetCallingAssembly();
            AddEasyBlazorNavigationManager<T>(services,
                new[] {assmebly},
                pagePostfixCheck,
                checkUnusedKeys);
        }
    }
}