using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

[assembly: InternalsVisibleTo("BlazorNexus.UnitTests")]

namespace BlazorNexsus.Navigation.Utils;

internal static class UriUtils
{
    public static string ReplaceRouteParams(string sourceRouteUri, IReadOnlyDictionary<string, string>? queryParams)
    {
     // var routeParamTemplate = @"{([a-zA-Z_]+)\??(?::\w+)?\/?}";
     var routeParamTemplate = @"{([a-zA-Z0-9]+)(\?)?(?::([a-zA-Z]+)(\?)?)?}";
     
        var result = Regex.Matches(sourceRouteUri, routeParamTemplate);
        foreach (Match match in result)
        {
            string? template = null;
            var param = match.Groups[1].Value;
            var sucess = queryParams?.TryGetValue(param, out template) ?? false;
            
            var isMandatory = match.Value.Contains("?}");
            
            if (!sucess && !isMandatory)
            {
                throw new Exception($"Parameter {param} is not found");
            }

            sourceRouteUri = sourceRouteUri.Replace(match.Value, template ?? string.Empty);
        }

        return sourceRouteUri.TrimEnd('/');
    }

    public static string ComposeUri(
        string uri,
        NavigationManager _navigationManager,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null) 
    {
        uri = ReplaceRouteParams(uri, navigationParams);
        
        if (queryParams != null)
        {
            var queryParamsTransformed = queryParams.Select(x =>
                new KeyValuePair<string, object?>(x.Key, x.Value)).ToDictionary();
            uri = _navigationManager.GetUriWithQueryParameters(uri, queryParamsTransformed);
        }

        return uri;
    }

    public static string ExtractQueryString(string uri)
    {
        var uriObj = new System.Uri(uri);
        return uriObj.Query.Replace("?", "");
    }

    public static string RemoveOptionalParams(string uri)
    {
        //{ClientId:guid?}
        //{ClientId:guid}
        //{ClientId?}
        //{ClientId}
        
        //Group0 ClientId
        //Group1 guid
        //Group2 ?
        var pattern = @"{([a-zA-Z0-9]+)(\?)?(?::([a-zA-Z]+)(\?)?)?}";
        MatchCollection matches = Regex.Matches(uri, pattern);
        
        foreach (Match match in matches)
        {
            var groups = ((IList<Group>) match.Groups)
                .Skip(1) // remove match, left only groups
                .Where(x => !string.IsNullOrEmpty(x.Value)).ToList();
            
            if (groups.Count == 3 ||  //{ClientId:guid?}
                (groups.Count == 2 && groups[1].Value == "?"))  //{ClientId?}
            {
                uri = uri.Replace(match.Value, string.Empty).TrimEnd('/'); //remove non-mandatory parameter
            }
            else
            {
                throw new Exception($"Parameter {match.Value} is not set");
            }
        }

        return uri;
    }

    public static T? ConvertValue<T>(string value)
    {
        var converter = TypeDescriptor.GetConverter(typeof (T));
        if (converter.CanConvertTo(typeof (T)) && converter.CanConvertFrom(typeof(string)))
        {
            return (T)converter.ConvertFromString(value)!;
        }
        else
        {
            return default(T?);
        }
    }
    
}