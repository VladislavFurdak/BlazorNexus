using BlazorNexsus.Navigation.DTOs;
using BlazorNexsus.Navigation.Models;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorNexsus.Navigation.Abstractions;

public interface INavigationManager<T> where T : struct, Enum
{
    Task Go(T pageKey, NexusNavigationOptions<T>? options = null);

    Task Go(
        T pageKey,
        bool newTab,
        T? backPage = null,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null);
    
    /// <summary>
    /// 
    /// </summary>
    bool HasBackPage { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fallBackPageKey"></param>
    /// <returns></returns>
    void Back(T fallBackPageKey, bool preserveQueryString = true);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task Refresh(bool browserReload);
    
    /// <summary>
    /// 
    /// </summary>
    T CurrentPage { get; }
    
    /// <summary>
    /// 
    /// </summary>
    IReadOnlyDictionary<T, string> Routes { get; }

    /// <summary>
    /// 
    /// </summary>
    Task<NavigationInfo<T>> PreviousPage { get; }

    /// <summary>
    /// 
    /// </summary>
    event EventHandler<LocationChangedEventArgs> LocationChanged;
}