using System.Text.RegularExpressions;
using BlazorNexsus.Navigation.Internal;
using Microsoft.AspNetCore.Components;

namespace BlazorNexsus.Navigation;

internal static class UriUtils
{
    private static string ReplaceRoutParams(string sourceRouteUri, IReadOnlyDictionary<string, string> queryParams)
    {
        var routeParamTemplate = @"{([a-zA-Z_]+)\??(?::\w+)?}";
        var result = Regex.Matches(sourceRouteUri, routeParamTemplate);
        foreach (Match match in result)
        {
            string? template = null;
            var sucess = queryParams.TryGetValue(match.Groups[1].Value, out template);

            if (!sucess)
            {
                throw new Exception($"Parameter {match.Groups[1].Value} is not found");
            }

            sourceRouteUri = sourceRouteUri.Replace(match.Value, template ?? string.Empty);
        }

        return sourceRouteUri.TrimEnd('/');
    }

    public static string GetUri<T>(
        IReadOnlyDictionary<T, RouteInfoDTO> _routes,
        NavigationManager _navigationManager,
        T pageKey,
        bool newTab = false,
        T? backPage = null,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null) where T : struct, Enum
    {
        var uri = _routes[pageKey].Route;

        if (navigationParams != null)
        {
            uri = ReplaceRoutParams(uri, navigationParams);
        }
        
        if (queryParams != null)
        {
            var queryParamsTransformed = queryParams.Select(x =>
                new KeyValuePair<string, object?>(x.Key, x.Value)).ToDictionary();
            uri = _navigationManager.GetUriWithQueryParameters(uri, queryParamsTransformed);
        }

        return uri;
    }
}