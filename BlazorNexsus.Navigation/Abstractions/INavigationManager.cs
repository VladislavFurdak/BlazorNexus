using BlazorNexsus.Navigation.DTOs;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorNexsus.Navigation.Abstractions;

public interface INavigationManager<T> where T : struct, Enum
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageKey"></param>
    /// <param name="navigationParams"></param>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    void Go(T pageKey);

    public void Go(
        T pageKey,
        T? backPage = null,
        Dictionary<string, string>? navigationParams = null,
        Dictionary<string, string>? queryParams = null);
    
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