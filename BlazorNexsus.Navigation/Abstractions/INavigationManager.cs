using BlazorNexsus.Navigation.DTOs;
using BlazorNexsus.Navigation.Models;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorNexsus.Navigation.Abstractions;

public interface INavigationManager<T> where T : struct, Enum
{
    void Go(T pageKey);
    
    Task Go(T pageKey, NexusNavigationOptions<T> options);

    Task Go(
        T pageKey,
        bool newTab = false,
        T? backPage = null,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null);

    /// <summary>
    /// 
    /// </summary>
    Task<bool> HasBackPage();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fallBackPageKey"></param>
    /// <returns></returns>
    Task Back(T fallBackPageKey, bool preserveQueryString = true);
    
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