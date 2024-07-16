﻿using Microsoft.AspNetCore.Components.Routing;

namespace BlazorNexsus.Navigation;

public interface INavigationManager<T> where T : struct, Enum
{
    /// <summary>
    /// Navigate to a page
    /// </summary>
    /// <param name="pageKey">Enum key of the page</param>
    void Go(T pageKey);
    
    Task Go(NexusNavigationOptions<T> options);

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
    event EventHandler<LocationChangedEventArgs> LocationChanged;
}