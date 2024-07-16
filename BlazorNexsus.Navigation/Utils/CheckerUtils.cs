using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace BlazorNexsus.Navigation;

public static class CheckerUtils
{
    public static void CheckPagesPostfixes(Assembly targetAssembly)
    {
        var pages = targetAssembly.GetTypes()
            .Where(x => !x.IsAbstract && x.InheritsFrom(typeof(ComponentBase)));

        var pagesWithRouting = pages
            .Where(x => x.GetCustomAttribute<RouteAttribute>() != null).ToList();

        var results = new List<string>();
        foreach (var page in pagesWithRouting)
        {
            if (!page.Name.EndsWith("Page"))
            {
                results.Add(page.Name);
            }
        }

        if (results.Any())
        {
            var listOfTypes = results.Select(x=>$"'{x}.razor'").Aggregate((a, b) => $"{a},{b}");
            throw new Exception($"[Naming Convention Error] Assembly:{targetAssembly.FullName} Types: {listOfTypes} should have 'Page' postfix in Type's name. To disable the error set pagePostfixCheck=false in AddBlazorNexusNavigation method");
        }
    }
    
    public static void CheckUnusedKeys<T>(Assembly targetAssembly) where T : struct, Enum
    {
        var pages = targetAssembly.GetTypes()
            .Where(x => !x.IsAbstract && x.InheritsFrom(typeof(ComponentBase)));

        var pagesWithRouting = pages
            .Where(x => x.GetCustomAttribute<RouteAttribute>() != null).Select(x=>x.Name).ToList();
        
        var enumStrings = Enum.GetValues<T>().Select(x=>x.ToString()).ToList();
        
        var results = enumStrings.Except(pagesWithRouting).ToList();
        
        if (results.Any())
        {
            var listOfTypes = results.Select(x=>$"'{x}'").Aggregate((a, b) => $"{a},{b}");
            throw new Exception($"[Unused Page Keys Found] Assembly:{targetAssembly.FullName}, Enum: '{typeof(T).Name}'. Page enum keys: {listOfTypes} are not used. To disable the error set checkUnusedKeys=false in AddBlazorNexusNavigation method");
        }
    }
}