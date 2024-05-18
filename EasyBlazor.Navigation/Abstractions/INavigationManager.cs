using System.Collections.Immutable;
using EasyBlazor.Navigation.DTOs;

namespace EasyBlazor.Navigation.Abstractions;

public interface INavigationManager<T> where T : struct, Enum
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageKey"></param>
    /// <param name="navigationParams"></param>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    void Go(T pageKey, 
        Dictionary<string, string>? navigationParams = null,
        Dictionary<string, string>? queryParams = null);

    public void GoWithBack(
        T pageKey,
        T backPage,
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
    void Back(T fallBackPageKey, bool includePreviousQueryString = true);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task Refresh();
    
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
    IReadOnlyList<NavigationInfo> History { get; }
    
    /// <summary>
    /// 
    /// </summary>
    Task<NavigationInfo> PreviousPage { get; }
}