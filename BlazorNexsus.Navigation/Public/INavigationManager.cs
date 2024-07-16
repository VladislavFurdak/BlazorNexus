using Microsoft.AspNetCore.Components.Routing;

namespace BlazorNexsus.Navigation;

public interface INavigationManager<T> where T : struct, Enum
{
    /// <summary>
    /// Navigate to another page.
    /// </summary>
    /// <param name="pageKey">Enum key of the page.</param>
    void Go(T pageKey);
    
    /// <summary>
    /// Navigate to another page.
    /// </summary>
    /// <param name="options">Keeps parameters of transition.</param>
    /// <returns></returns>
    Task Go(NexusNavigationOptions<T> options);

    /// <summary>
    /// Navigate to another page, using defined settings.
    /// </summary>
    /// <param name="pageKey">The page which you want to navigate.</param>
    /// <param name="newTab">Is the page should be opened in new the browser tab.</param>
    /// <param name="backPage">Set up a "back" page, to be able to call "back" method after.</param>
    /// <param name="navigationParams">Defines the list of parameters for standard Blazor route templates.</param>
    /// <param name="queryParams">Defines the list of query string parameters.</param>
    /// <returns></returns>
    Task Go(
        T pageKey,
        bool newTab = false,
        T? backPage = null,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null);

    /// <summary>
    /// Shows is "back" page has been set up earlier.
    /// </summary>
    Task<bool> HasBackPage();

    /// <summary>
    /// Calls "back" action, in case you have the "back" page set previously using the Go method.
    /// </summary>
    /// <param name="fallBackPageKey"></param>
    /// <returns></returns>
    Task Back(T fallBackPageKey, bool preserveQueryString = true);
    
    /// <summary>
    /// Refresh the page.
    /// </summary>
    /// <param name="browserReload">Reload browser tab.</param>
    /// <returns></returns>
    Task Refresh(bool browserReload);
    
    /// <summary>
    /// Returns the current page.
    /// </summary>
    T CurrentPage { get; }
    
    /// <summary>
    /// A collection of routes.
    /// </summary>
    IReadOnlyDictionary<T, string> Routes { get; }

    /// <summary>
    /// Triggered when URI has been changed.
    /// </summary>
    event EventHandler<LocationChangedEventArgs> LocationChanged;

    /// <summary>
    /// Get a parameter from Browsers's Query String.
    /// </summary>
    /// <param name="key">The Key of parameter.</param>
    /// <typeparam name="PType">The Type of parameter.</typeparam>
    /// <returns></returns>
    public PType? GetQueryStringParam<PType>(string key);
}