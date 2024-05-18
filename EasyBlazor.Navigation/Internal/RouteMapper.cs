using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace EasyBlazor.Navigation.Internal;

public static class RouteMapper
{
    public static IReadOnlyDictionary<T, RouteInfoDTO> MapRoutes<T>(IEnumerable<Assembly> lookupAssemblies) 
        where T : struct, Enum
    {
        var pages = lookupAssemblies.SelectMany(x => x.GetTypes())
            .Where(x => !x.IsAbstract && x.InheritsFrom(typeof(ComponentBase)));

        var pagesWithRouting = pages
            .Where(x => x.GetCustomAttribute<RouteAttribute>() != null).ToList();

        var enumStrings = Enum.GetValues<T>().ToDictionary(x => x.ToString());
        
        var result = new Dictionary<T, RouteInfoDTO>();

        foreach (var page in pagesWithRouting)
        {
            var routing = enumStrings.Where(x => x.Key == page.Name).ToList();

            switch (routing.Count)
            {
                case 0:
                    //TODO no routing for page warning
                    break;
                case > 1:
                    //TODO multiple page for rout ID error
                    break;
                case 1:
                    var enumValue = routing.First().Value;
                    var pageRoutingString = page.GetCustomAttribute<RouteAttribute>()?.Template;
                    if (pageRoutingString is not null)
                    {
                        var routeDto = new RouteInfoDTO
                        {
                            Route = pageRoutingString,
                            RouteSegments = pageRoutingString.Split("/")
                                .Where(x => !string.IsNullOrEmpty(x)).ToList()
                        };
                        
                        result[enumValue] = routeDto;
                    }
                    else
                    {
                        //TODO routing is null
                    }
                    break;
            }
        }

        return result;
    }
}