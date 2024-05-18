using System.Text.RegularExpressions;

namespace EasyBlazor.Navigation.Internal;

public static class PathMapper
{
    public static T GetPageKeyFromRoute<T>(IReadOnlyDictionary<T, RouteInfoDTO> routes, string UriSegments) where T : Enum
    {
        foreach (var route in routes)
        {
            if (route.Value.RouteMatchesUri(UriSegments))
            {
                return route.Key;
            }
        }

        throw new Exception("route didn't found");
    }
}