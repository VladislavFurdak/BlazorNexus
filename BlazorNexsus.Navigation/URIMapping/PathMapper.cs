namespace BlazorNexsus.Navigation.URIMapping;

internal static class PathMapper
{
    public static T GetPageKeyFromRoute<T>(IReadOnlyDictionary<T, RouteInfoDTO> routes, string uri) where T : Enum
    {
        foreach (var route in routes)
        {
            if (route.Value.RouteMatchesUri(uri))
            {
                return route.Key;
            }
        }

        throw new Exception($"The route '{uri}' didn't found");
    }
}